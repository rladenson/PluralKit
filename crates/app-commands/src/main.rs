use twilight_model::application::command::CommandType;
use twilight_util::builder::command::{
    BooleanBuilder, CommandBuilder, StringBuilder, SubCommandBuilder, SubCommandGroupBuilder,
    UserBuilder,
};

#[libpk::main]
async fn main() -> anyhow::Result<()> {
    let discord = twilight_http::Client::builder()
        .token(libpk::config.discord().bot_token.clone())
        .build();

    let interaction = discord.interaction(twilight_model::id::Id::new(
        libpk::config.discord().client_id.clone().get(),
    ));

    let commands = vec![
        // message commands
        // description must be empty string
        CommandBuilder::new("\u{2753} Message info", "", CommandType::Message).build(),
        CommandBuilder::new("\u{274c} Delete message", "", CommandType::Message).build(),
        CommandBuilder::new("\u{1f514} Ping author", "", CommandType::Message).build(),
        // slash commands
        CommandBuilder::new(
            "system",
            "Commands run on a PK system",
            CommandType::ChatInput
        )
        .option(
            SubCommandBuilder::new(
                "new",
                "Makes a new PK system if one is not already on your account",
            )
            .option(StringBuilder::new("name", "The name of the new system"))
            .build()
        )
        .option(
            SubCommandBuilder::new(
                "info",
                "Show information about a PK system, defaults to the one on the current account if no info given"
            )
            .option(StringBuilder::new("id", "ID of system or discord account to fetch system of"))
            .option(UserBuilder::new("account", "Discord account to fetch the system of"))
            .build()
        )
        .option(SubCommandGroupBuilder::new("tag", "Set, clear, or view a system's tag")
            .subcommands([
                SubCommandBuilder::new("show", "View a system's tag")
                    .option(StringBuilder::new("id", "ID of system or discord account to view tag of"))
                    .option(UserBuilder::new("account", "Discord account to view tag of"))
                    .option(BooleanBuilder::new("server-specific", "Set to true if you want to view the system's servertag instead of global tag")),
                SubCommandBuilder::new("set", "Set your system tag")
                    .option(BooleanBuilder::new("server-specific", "Set to true if you want to set your system's servertag instead of global tag")),
                SubCommandBuilder::new("clear", "Clear your system's tag")
                    .option(BooleanBuilder::new("server-specific", "Set to true if you want to clear your system's servertag instead of global tag")),
            ])
            .build()
        )
        .build(),
    ];

    interaction.set_global_commands(&commands).await?;

    Ok(())
}
