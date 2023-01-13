using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;

namespace Overlewd
{
    //alchemy
    public class Alchemy : BaseGameMeta
    {
        public List<AdminBRO.AlchemyIngredient> ingredients { get; private set; } = new List<AdminBRO.AlchemyIngredient>();
        public List<AdminBRO.AlchemyMixture> mixture { get; private set; } = new List<AdminBRO.AlchemyMixture>();
        public List<AdminBRO.AlchemyRecipe> recipe { get; private set; } = new List<AdminBRO.AlchemyRecipe>();
        public override async Task Get()
        {
            ingredients = await AdminBRO.alchemyIngredientsAsync();
            mixture = await AdminBRO.alchemyMixturesAsync();
            recipe = await AdminBRO.alchemyRecipesAsync();
        }

        public async Task<AdminBRO.BrewResult> Brew(int[] ingredientIds)
        {
            return await AdminBRO.alchemyBrewAsync(ingredientIds);
        }
    }

    //boss mini game
    public class BossMiniGame : BaseGameMeta
    {
        public List<AdminBRO.MiniGame> bossMiniGames { get; private set; } = new List<AdminBRO.MiniGame>();
        public override async Task Get()
        {
            bossMiniGames = await AdminBRO.miniGamesAsync();
        }

        public AdminBRO.MiniGame GetMiniGameDataByEventId(int eventId) =>
            bossMiniGames.Find(mg => mg.eventId == eventId);

        public async Task<bool> MiniGameEnabled(int eventStageId)
        {
            var result = await AdminBRO.miniGameEnabledAsync(eventStageId);
            return result.dData.isActive;
        }

        public async Task<int> MiniGameChance(int eventId)
        {
            var result = await AdminBRO.miniGameChanceAsync(eventId);
            return result.dData.chance;
        }

        public async Task MiniGameEnd(int miniGameBattleId, AdminBRO.BattleEndData battleEndData = null) =>
            await AdminBRO.miniGameEndAsync(miniGameBattleId, battleEndData);
    }
}