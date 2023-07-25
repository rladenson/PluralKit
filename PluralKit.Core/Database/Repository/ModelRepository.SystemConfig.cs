using SqlKata;

namespace PluralKit.Core;

public partial class ModelRepository
{
    public async Task<SystemConfig> GetSystemConfig(SystemId system, IPKConnection conn = null)
    {
        var cfg = await _db.QueryFirst<SystemConfig>(conn, new Query("system_config").Where("system", system));
        var trusted = await _db.Query<ulong>(new Query("trusted_users").Select("uid").Where("system", system));
        cfg.TrustedUsers = trusted;
        return cfg;
    }

    public async Task<SystemConfig> UpdateSystemConfig(SystemId system, SystemConfigPatch patch, IPKConnection conn = null)
    {
        var query = patch.Apply(new Query("system_config").Where("system", system));
        var config = await _db.QueryFirst<SystemConfig>(conn, query, "returning *");

        _ = _dispatch.Dispatch(system, new UpdateDispatchData
        {
            Event = DispatchEvent.UPDATE_SETTINGS,
            EventData = patch.ToJson()
        });

        return config;
    }

    public async Task AddTrusted(SystemId system, ulong accountId, IPKConnection conn = null)
    {
        var query = new Query("trusted_users").AsInsert(new { system, uid = accountId });
        await _db.ExecuteQuery(conn, query, "on conflict (system, uid) do update set system = @p0");

        _ = _dispatch.Dispatch(system, new UpdateDispatchData
        {
            Event = DispatchEvent.ADD_TRUSTED_USER,
            EntityId = accountId.ToString(),
        });
    }

    public async Task RemoveTrusted(SystemId system, ulong accountId)
    {
        var query = new Query("trusted_users").AsDelete().Where("system", system).Where("uid", accountId);
        await _db.ExecuteQuery(query);

        _ = _dispatch.Dispatch(system, new UpdateDispatchData
        {
            Event = DispatchEvent.REMOVE_TRUSTED_USER,
            EntityId = accountId.ToString(),
        });
    }

    public async Task<bool> GetTrustedRelation(SystemId system, ulong accountId)
    {
        var query = new Query("trusted_users").Select("*").Where("system", system).Where("uid", accountId);
        var relation = await _db.ExecuteQuery(query);
        if (relation > 0)
            return true;
        return false;
    }
}