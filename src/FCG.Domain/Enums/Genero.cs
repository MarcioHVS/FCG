using System.Text.Json.Serialization;

namespace FCG.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Genero
    {
        Acao,
        Aventura,
        RPG,
        FPS,
        TPS,
        Plataforma,
        Luta,
        Corrida,
        Esportes,
        Estrategia,
        SurvivalHorror,
        BattleRoyale,
        Simulacao,
        Roguelike,
        MMORPG,
        Metroidvania,
        Puzzle,
        Idle,
        DeckBuilding,
        Sandbox
    }
}
