using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Linq;
using TMPro;
using System.Text;

namespace Overlewd
{
    public static partial class AdminBRO
    {
        // /alchemy
        // /alchemy/my/ingredients
        // /alchemy/my/mixtures
        // /alchemy/recipes
        // /alchemy/brew
        public static async Task<HttpCoreResponse<List<AlchemyIngredient>>> alchemyIngredientsAsync() =>
            await HttpCore.GetAsync<List<AlchemyIngredient>>(make_url("alchemy/my/ingredients"));
        public static async Task<HttpCoreResponse<List<AlchemyMixture>>> alchemyMixturesAsync() =>
            await HttpCore.GetAsync<List<AlchemyMixture>>(make_url("alchemy/my/mixtures"));
        public static async Task<HttpCoreResponse<List<AlchemyRecipe>>> alchemyRecipesAsync() =>
            await HttpCore.GetAsync<List<AlchemyRecipe>>(make_url("alchemy/recipes"));
        public static async Task<HttpCoreResponse<BrewResult>> alchemyBrewAsync(int[] ingredientIds)
        {
            var form = new WWWForm();
            foreach (var iId in ingredientIds)
            {
                form.AddField("ingredientIds[]", iId);
            }
            return await HttpCore.PostAsync<BrewResult>(make_url("alchemy/brew"), form);
        }

        [Serializable]
        public class AlchemyIngredient
        {
            public int ingredientId;
            public int amount;
            public string name;
            public int dropChance;
            public int dropChanceBoss;
            public string icon;
        }

        [Serializable]
        public class AlchemyMixture
        {
            public int mixtureId;
            public int amount;
            public string name;
            public int magnitude;
            public string mixtureType;
            public string effectDescription;
            public string icon;
        }

        [Serializable]
        public class AlchemyRecipe
        {
            public int recipeId;
            public string recipeName;
            public List<int> ingredientIds;
            public int resultMixtureId;
        }

        [Serializable]
        public class BrewResult
        {
            public string result;
            public int? usedRecipeId;
            public int? resultMixtureId;
        }

        // /battles/mini-game
        // /battles/mini-game-enabled/{eventStageId}
        // /battles/mini-game-chance/{eventId}
        // /battles/mini-games
        // /battles/mini-game/{miniGameBattleId}/end
        public static async Task<HttpCoreResponse<MiniGameEnabled>> miniGameEnabledAsync(int eventStageId) =>
            await HttpCore.GetAsync<MiniGameEnabled>(make_url($"battles/mini-game-enabled/{eventStageId}"));
        public static async Task<HttpCoreResponse<MiniGameChance>> miniGameChanceAsync(int eventId) =>
            await HttpCore.GetAsync<MiniGameChance>(make_url($"battles/mini-game-chance/{eventId}"));
        public static async Task<HttpCoreResponse<List<MiniGame>>> miniGamesAsync() =>
            await HttpCore.GetAsync<List<MiniGame>>(make_url("battles/mini-games"));
        public static async Task<HttpCoreResponse<List<GenRewardItem>>> miniGameEndAsync(int miniGameBattleId, BattleEndData battleEndData = null) =>
            await HttpCore.PostAsync<List<GenRewardItem>>(make_url($"battles/mini-game/{miniGameBattleId}/end"), battleEndData?.ToWWWForm());

        [Serializable]
        public class MiniGameEnabled
        {
            public bool isActive;
            public bool isRewardEnabled;
        }

        [Serializable]
        public class MiniGameChance
        {
            public int chance;
        }

        [Serializable]
        public class MiniGame
        {
            public string name;
            public int eventId;
            public List<Battle> battles;

            public class Battle
            {
                public int id;
                public string title;
                public List<RewardItem> rewards;
                public List<BattlePhase> battlePhases;
                public float? requiredTeamPotency;
            }
            public class BattlePhase
            {
                public List<Character> enemyCharacters;
            }

            public Battle GetBattleByTeamPotency(float temPotency) =>
                battles.OrderByDescending(b => b.requiredTeamPotency).
                Where(b => temPotency >= b.requiredTeamPotency).FirstOrDefault();
        }
    }
}
