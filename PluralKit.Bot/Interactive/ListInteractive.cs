using Autofac;

using Myriad.Cache;
using Myriad.Gateway;
using Myriad.Rest.Types;
using Myriad.Types;

using PluralKit.Core;

namespace PluralKit.Bot.Interactive;

public class ListInteractive: BaseInteractive
{
    public ListInteractive(Context ctx) : base(ctx)
    {
        User = ctx.Author.Id;
    }

    public bool? Result { get; private set; }
    public ulong? User { get; set; }
    public ButtonStyle Style { get; set; } = ButtonStyle.Primary;
    public int Page { get; set; } = 0;
    public int PageCount { get; set; } = 1;
    public Func<int, Task<MessageComponent[]>> ContentBuilder { get; set; }
    public string? Color { get; set; }

    public override async Task Start()
    {
        if (PageCount > 1)
        {
            AddButton(ctx => OnButtonClick(ctx, "first"), "\u23EA", Style); // <<
            AddButton(ctx => OnButtonClick(ctx, "previous"), "\u2B05", Style); // <
            AddButton(ctx => OnButtonClick(ctx, "next"), "\u27A1", Style); // >
            AddButton(ctx => OnButtonClick(ctx, "last"), "\u23E9", Style); // >>
        }

        AddButton(ctx => OnButtonClick(ctx, "close"), Emojis.Error, Style);

        await Send();
    }

    private async Task OnButtonClick(InteractionContext ctx, String buttonAction)
    {
        if (ctx.User.Id != User)
        {
            // todo make silent instead of error?
            await Error(ctx, Errors.InteractionWrongAccount(User ?? 0));
            return;
        }

        switch (buttonAction)
        {
            case "first":
                Page = 0;
                await Update(ctx);
                break;
            case "previous":
                Page = (Page - 1) % PageCount;
                if (Page < 0) Page += PageCount;
                await Update(ctx);
                break;
            case "next":
                Page = (Page + 1) % PageCount;
                await Update(ctx);
                break;
            case "last":
                Page = PageCount - 1;
                await Update(ctx);
                break;
            case "close":
                await Finish(ctx);
                break;
        }
    }

    // The generic method we're overriding has a parameter
    // But we'll never need it here
    public async override Task<MessageComponent[]> GetComponents(string? _)
    {
        var generatedComponents = await ContentBuilder(Page);
        return
        [
            new MessageComponent()
            {
                Type = ComponentType.Container,
                AccentColor = Color.ToDiscordColor() ?? null,
                Components =
              [
                generatedComponents[0],
                  generatedComponents[1],
                  new MessageComponent()
                  {
                      Type = ComponentType.Separator
                  },
                  new()
                  {
                      Type = ComponentType.ActionRow,
                      Components = _buttons.Select(b => b.ToMessageComponent()).ToArray()
                  },
                  generatedComponents[2]
              ]
            },
        ];
    }

}