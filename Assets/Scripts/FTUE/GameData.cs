using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Overlewd
{
    namespace FTUE
    {
        enum AnimationId
        {
            //emotion
            OverlordAngry,
            OverlordHappy,
            OverlordIdle,
            OverlordLove,
            OverlordSurprised,

            UlviAngry,
            UlviHappy,
            UlviIdle,
            UlviLove,
            UlviSurprised,

            UlviWolfAngry,
            UlviWolfHappy,
            UlviWolfIdle,
            UlviWolfLove,
            UlviWolfSurprised,

            AdrielAngry,
            AdrielHappy,
            AdrielIdle,
            AdrielLove,
            AdrielSurprised,

            //cutIn
            CutIn1,
            CutIn2,
            CutIn3,
            CutIn4,
            CutIn5,
            CutIn6,
            CutIn7,
            CutIn8,
            CutIn9,

            //main
            MainSex1,
            FinalSex1,

            MainSex2,
            FinalSex2,

            MainSex3,
            FinalSex3,
        }

        class EmotionAnimationName
        {
            public const string Angry = "angry";
            public const string Happy = "happy";
            public const string Idle = "idle";
            public const string Love = "love";
            public const string Surprised = "surprised";
        }

        public static class GameData
        {
            public static AdminBRO.Dialog GetSexById(int id)
            {
                if (GameGlobalStates.newFTUE)
                {
                    return Overlewd.GameData.GetDialogById(id);
                }
                return sexs.Find(d => d.id == id);
            }
            public static List<AdminBRO.Dialog> sexs = new List<AdminBRO.Dialog>();

            public static AdminBRO.Dialog GetDialogById(int id) 
            {
                if (GameGlobalStates.newFTUE)
                {
                    return Overlewd.GameData.GetDialogById(id);
                }
                return dialogs.Find(d => d.id == id);
            }
            public static List<AdminBRO.Dialog> dialogs = new List<AdminBRO.Dialog>();

            public static AdminBRO.Dialog GetNotificationById(int id)
            {
                if (GameGlobalStates.newFTUE)
                {
                    return Overlewd.GameData.GetDialogById(id);
                }
                return notifications.Find(d => d.id == id);
            }
            public static List<AdminBRO.Dialog> notifications = new List<AdminBRO.Dialog>();

            public static AdminBRO.Animation GetAnimationById(int id)
            {
                if (GameGlobalStates.newFTUE)
                {
                    return Overlewd.GameData.GetAnimationById(id);
                }
                return animations.Find(a => a.id == id);
            }
            public static List<AdminBRO.Animation> animations = new List<AdminBRO.Animation>();

            public static string[] castleScreenHints =
            { 
                "empty",
                "Stripping the castle surroundings",
                "Build Ulvi's house",
                "Enslave Lunaria Realm",
                "Build Portal",
                "Enslave Lunaria Realm"
            };

            private static bool _initialization = false;
            public static void Initialization()
            {
                if (_initialization)
                    return;
                _initialization = true;

                //sexs
                //SEX 1
                sexs.Add(new AdminBRO.Dialog
                {
                    id = 1,
                    replicas = new List<AdminBRO.DialogReplica>
                    {
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            message = "I can't believe I found him! But… How do I wake him up?",
                            emotionAnimationId = (int)AnimationId.UlviSurprised,
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            message = "Well, at least one part of him seems to be up. I can work with that!",
                            emotionAnimationId = (int)AnimationId.UlviLove
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            mainSoundPath = FMODEventPath.SexScene1_MainScene,
                            message = "You're finally awake! Took you long enough, Master.",
                            emotionAnimationId = (int)AnimationId.UlviHappy,
                            mainAnimationId = (int)AnimationId.MainSex1
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            mainSoundPath = FMODEventPath.SexScene1_MainScene,
                            message = "M-m-m, your cock tastes so good... I guess the legends were true.",
                            emotionAnimationId = (int)AnimationId.UlviLove,
                            mainAnimationId = (int)AnimationId.MainSex1
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            mainSoundPath = FMODEventPath.SexScene1_MainScene,
                            cutInSoundPath = FMODEventPath.SexScene1_CutInLick,
                            message = "I can lick your cock for hours. It feels so smooth and silky...",
                            emotionAnimationId = (int)AnimationId.UlviLove,
                            cutInAnimationId = (int)AnimationId.CutIn1,
                            mainAnimationId = (int)AnimationId.MainSex1,
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            mainSoundPath = FMODEventPath.SexScene1_MainScene,
                            cutInSoundPath = FMODEventPath.SexScene1_CutInLick,
                            message = "You're so big. I can barely fit you inside my tight mouth.",
                            emotionAnimationId = (int)AnimationId.UlviLove,
                            cutInAnimationId = (int)AnimationId.CutIn1,
                            mainAnimationId = (int)AnimationId.MainSex1,
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            mainSoundPath = FMODEventPath.SexScene1_MainScene,
                            message = "Please, cum for me, Master! I want to taste you so bad…",
                            emotionAnimationId = (int)AnimationId.UlviLove,
                            mainAnimationId = (int)AnimationId.MainSex1,
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            mainSoundPath = FMODEventPath.SexScene1_MainScene,
                            cutInSoundPath = FMODEventPath.SexScene1_CutInCumshot,
                            message = "Thank you, Master. I'm so happy!",
                            emotionAnimationId = (int)AnimationId.UlviHappy,
                            cutInAnimationId = (int)AnimationId.CutIn2,
                            mainAnimationId = (int)AnimationId.MainSex1,
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            mainSoundPath = FMODEventPath.SexScene1_MainScene,
                            cutInSoundPath = FMODEventPath.SexScene1_CutInCumshot,
                            message = "You taste exactly how I imagined. After all those years searching for you…",
                            emotionAnimationId = (int)AnimationId.UlviLove,
                            cutInAnimationId = (int)AnimationId.CutIn2,
                            mainAnimationId = (int)AnimationId.MainSex1,
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            message = "Now put your pants on, and let's get cracking! We have a realm to conquer.",
                            emotionAnimationId = (int)AnimationId.UlviHappy,
                            mainAnimationId = (int)AnimationId.FinalSex1,
                        },
                    }
                });

                //SEX 2
                sexs.Add(new AdminBRO.Dialog
                {
                    id = 2,
                    replicas = new List<AdminBRO.DialogReplica>()
                    {
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            mainSoundPath = FMODEventPath.SexScene2_MainScene,
                            message = "Master, watching you annihilate your foes has made me so horny…",
                            emotionAnimationId = (int)AnimationId.UlviLove,
                            mainAnimationId = (int)AnimationId.MainSex2,
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            mainSoundPath = FMODEventPath.SexScene2_MainScene,
                            cutInSoundPath = FMODEventPath.SexScene2_CutInBeads,
                            message = "Ah~ You pulled my tail out! I'm so embarrassed… But it feels so good!",
                            emotionAnimationId = (int)AnimationId.UlviLove,
                            cutInAnimationId = (int)AnimationId.CutIn8,
                            mainAnimationId = (int)AnimationId.MainSex2,
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            mainSoundPath = FMODEventPath.SexScene2_MainScene,
                            cutInSoundPath = FMODEventPath.SexScene2_CutInBeads,
                            message = "I've been preparing all my holes for you to fill. I think I'm addicted to your cock.",
                            emotionAnimationId = (int)AnimationId.UlviLove,
                            cutInAnimationId = (int)AnimationId.CutIn8,
                            mainAnimationId = (int)AnimationId.MainSex2,
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            mainSoundPath = FMODEventPath.SexScene2_MainScene,
                            cutInSoundPath = FMODEventPath.SexScene2_CutInBeads,
                            message = "Ummh~ I want to take your hot load… Cum with me, Master!",
                            emotionAnimationId = (int)AnimationId.UlviLove,
                            cutInAnimationId = (int)AnimationId.CutIn8,
                            mainAnimationId = (int)AnimationId.MainSex2,
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            mainSoundPath = FMODEventPath.SexScene2_MainScene,
                            cutInSoundPath = FMODEventPath.SexScene2_CutInCreamPie,
                            message = "Aahh~ Aahh! Just like that!",
                            emotionAnimationId = (int)AnimationId.UlviLove,
                            cutInAnimationId = (int)AnimationId.CutIn9,
                            mainAnimationId = (int)AnimationId.MainSex2,
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            mainSoundPath = FMODEventPath.SexScene2_MainScene,
                            cutInSoundPath = FMODEventPath.SexScene2_CutInCreamPie,
                            message = "Mmm~ Fuck yes!",
                            emotionAnimationId = (int)AnimationId.UlviLove,
                            cutInAnimationId = (int)AnimationId.CutIn9,
                            mainAnimationId = (int)AnimationId.MainSex2,
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            mainSoundPath = FMODEventPath.SexScene2_MainScene,
                            cutInSoundPath = FMODEventPath.SexScene2_CutInCreamPie,
                            message = "Your load feels so hot on my pussy… Thank you for treating me, Master.",
                            emotionAnimationId = (int)AnimationId.UlviLove,
                            cutInAnimationId = (int)AnimationId.CutIn9,
                            mainAnimationId = (int)AnimationId.MainSex2,
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            mainSoundPath = FMODEventPath.SexScene2_FinalScene,
                            message =
                                "Please, don't forget to play with me often. All of my holes are ready to serve you whenever you want!",
                            emotionAnimationId = (int)AnimationId.UlviLove,
                            mainAnimationId = (int)AnimationId.FinalSex2,
                        }
                    }
                });

                //SEX 3
                sexs.Add(new AdminBRO.Dialog
                {
                    id = 3,
                    replicas = new List<AdminBRO.DialogReplica>
                    {
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            message = "Your nimble fingers playing with my pussy make me so horny…",
                            emotionAnimationId = (int)AnimationId.UlviLove,
                            mainAnimationId = (int)AnimationId.MainSex3,
                            mainSoundPath = FMODEventPath.SexScene3_MainScene
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            message = "Is this what the touch of a Master feels like?",
                            emotionAnimationId = (int)AnimationId.UlviLove,
                            cutInAnimationId = (int)AnimationId.CutIn6,
                            cutInSoundPath = FMODEventPath.SexSscene3_CutInPussyRubbing
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            message = "My clit is throbbing under your touch, it feels so good!",
                            emotionAnimationId = (int)AnimationId.UlviLove,
                            cutInAnimationId = (int)AnimationId.CutIn6,
                            cutInSoundPath = FMODEventPath.SexSscene3_CutInPussyRubbing
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            message = "Ahh~ I wish you'd fuck me already and make me yours forever.",
                            emotionAnimationId = (int)AnimationId.UlviLove,
                            cutInAnimationId = (int)AnimationId.CutIn6,
                            cutInSoundPath = FMODEventPath.SexSscene3_CutInPussyRubbing
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            message = "Umm~ I've creamed all over your fingers, oops.",
                            emotionAnimationId = (int)AnimationId.UlviLove,
                            cutInAnimationId = (int)AnimationId.CutIn7,
                            cutInSoundPath = FMODEventPath.SexScene3_CutInPussyRubbingZoom
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            message = "Now your cock will slide in so easily… And you can be my Master.",
                            emotionAnimationId = (int)AnimationId.UlviLove,
                            cutInAnimationId = (int)AnimationId.CutIn7,
                            cutInSoundPath = FMODEventPath.SexScene3_CutInPussyRubbingZoom
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            message = "What do you mean you're not ready? I was ready! I was a good girl! I… Wasn't I?",
                            emotionAnimationId = (int)AnimationId.UlviSurprised,
                            cutInAnimationId = (int)AnimationId.CutIn7,
                            cutInSoundPath = FMODEventPath.SexScene3_CutInPussyRubbingZoom
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            message = "Fine! Walk away. No — fuck off forever! I know how my Master will feel like. He'd fuck me so much better!",
                            emotionAnimationId = (int)AnimationId.UlviAngry,
                            mainAnimationId = (int)AnimationId.FinalSex3,
                            mainSoundPath = FMODEventPath.SexScene3_FinalScene
                        },
                    }
                });

                //dialogs
                //STORY 1
                dialogs.Add(new AdminBRO.Dialog
                {
                    id = 1,
                    replicas = new List<AdminBRO.DialogReplica>
                    {
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            characterSkin = AdminBRO.DialogCharacterSkin.Ulvi,
                            message =
                                "I'm so happy I found you, Master! Wolves without a master are like humans without arms.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right,
                            emotionAnimationId = (int)AnimationId.UlviHappy,
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Overlord,
                            characterSkin = AdminBRO.DialogCharacterSkin.Overlord,
                            message = "So why did you pick <b>me?</b>",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left,
                            emotionAnimationId = (int)AnimationId.OverlordIdle,
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            characterSkin = AdminBRO.DialogCharacterSkin.Ulvi,
                            message =
                                "Ah… All those stories about you fascinated me. I want to help you retake what's yours!",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right,
                            emotionAnimationId = (int)AnimationId.UlviHappy,
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Overlord,
                            characterSkin = AdminBRO.DialogCharacterSkin.Overlord,
                            message = "You're a good girl, Ulvi.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left,
                            emotionAnimationId = (int)AnimationId.OverlordIdle,
                            cutInAnimationId = (int)AnimationId.CutIn3,
                            cutInSoundPath = FMODEventPath.Dialogue1_CutInUlviScratch
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            characterSkin = AdminBRO.DialogCharacterSkin.Ulvi,
                            message = "A hundred years of searching for you has been worth it.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right,
                            emotionAnimationId = (int)AnimationId.UlviHappy,
                            cutInAnimationId = (int)AnimationId.CutIn3,
                            cutInSoundPath = FMODEventPath.Dialogue1_CutInUlviScratch
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Overlord,
                            characterSkin = AdminBRO.DialogCharacterSkin.Overlord,
                            message =
                                "A hundred years? Sheesh. I don't even remember anything that happened before I fell asleep.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left,
                            emotionAnimationId = (int)AnimationId.OverlordIdle,
                            cutInAnimationId = (int)AnimationId.CutIn3,
                            cutInSoundPath = FMODEventPath.Dialogue1_CutInUlviScratch
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            characterSkin = AdminBRO.DialogCharacterSkin.Ulvi,
                            message =
                                "Oh, it's okay, Master. We'll get your memories back! Magical slumber tends to have an amnesia-like effect.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right,
                            emotionAnimationId = (int)AnimationId.UlviIdle,
                        },
                    }
                });

                //STORY 2
                dialogs.Add(new AdminBRO.Dialog
                {
                    id = 2,
                    replicas = new List<AdminBRO.DialogReplica>
                    {
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            characterSkin = AdminBRO.DialogCharacterSkin.Ulvi,
                            message = "You really are the guy from all those legends I heard my whole life! Strong, determined, bloodthirsty.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right,
                            emotionAnimationId = (int)AnimationId.UlviSurprised,
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Overlord,
                            characterSkin = AdminBRO.DialogCharacterSkin.Overlord,
                            message = "Are there legends about me? <b>Sweet.</b> Tell me more.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left,
                            emotionAnimationId = (int)AnimationId.OverlordIdle,
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            characterSkin = AdminBRO.DialogCharacterSkin.Ulvi,
                            message = "Oh, you were <b>the Big Bad!</b> When you approached with your armies, children cried, soldiers shat themselves. It was great! The Empress herself was involved. She became your main rival.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right,
                            emotionAnimationId = (int)AnimationId.UlviHappy,
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            characterSkin = AdminBRO.DialogCharacterSkin.Ulvi,
                            message = "Man, she hated you so much! And what she hated the most was that she was so horny for you!",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right,
                            emotionAnimationId = (int)AnimationId.UlviHappy,
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Overlord,
                            characterSkin = AdminBRO.DialogCharacterSkin.Overlord,
                            message = "Horny? Huh. Before we get sidetracked — why was I even asleep when you found me?",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left,
                            emotionAnimationId = (int)AnimationId.OverlordIdle,
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            characterSkin = AdminBRO.DialogCharacterSkin.Ulvi,
                            message = "Uhh… The thing is… You really wanna know?",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right,
                            emotionAnimationId = (int)AnimationId.UlviSurprised,
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Overlord,
                            characterSkin = AdminBRO.DialogCharacterSkin.Overlord,
                            message = "You have to tell me. Or I'll have to punish you.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left,
                            emotionAnimationId = (int)AnimationId.OverlordAngry,
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            characterSkin = AdminBRO.DialogCharacterSkin.Ulvi,
                            message = "Ah, you know I'm <b>a sucker</b> for punishment! Okay, so that's what I heard...",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right,
                            emotionAnimationId = (int)AnimationId.UlviLove,
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            characterSkin = AdminBRO.DialogCharacterSkin.Ulvi,
                            message = "You just raided one of the rival camps and were coming back to your stronghold with all the heavy equipment. So the old bridge across the moat broke under your weight.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right,
                            emotionAnimationId = (int)AnimationId.UlviIdle,
                            cutInAnimationId = (int)AnimationId.CutIn4,
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            characterSkin = AdminBRO.DialogCharacterSkin.Ulvi,
                            message = "Your camp was destroyed and you were captured. Unconcious and covered in sludge. The Empress' people brushed you off a bit and transported to the castle. I wish it was more flattering... ",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right,
                            emotionAnimationId = (int)AnimationId.UlviIdle,
                            cutInAnimationId = (int)AnimationId.CutIn4,
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Overlord,
                            characterSkin = AdminBRO.DialogCharacterSkin.Overlord,
                            message = "At least that explains the fishy smell.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left,
                            emotionAnimationId = (int)AnimationId.OverlordIdle,
                            cutInAnimationId = (int)AnimationId.CutIn4,
                        },
                    }
                });

                //STORY 3 //Story dialogue regarding the portal
                dialogs.Add(new AdminBRO.Dialog
                {
                    id = 3,
                    replicas = new List<AdminBRO.DialogReplica>
                    {
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            characterSkin = AdminBRO.DialogCharacterSkin.Ulvi,
                            message = "Master, I think we're ready to expand. This location would be <b>perfect</b> for a new camp!",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right,
                            emotionAnimationId = (int)AnimationId.UlviHappy,
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Adriel,
                            characterSkin = AdminBRO.DialogCharacterSkin.Adriel,
                            message = "This location is in my domain, dog. I'd rather it remain campless. What do we have here?",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right,
                            emotionAnimationId = (int)AnimationId.AdrielAngry,
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Overlord,
                            characterSkin = AdminBRO.DialogCharacterSkin.Overlord,
                            message = "Whoa. Hey there, beautiful.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left,
                            emotionAnimationId = (int)AnimationId.OverlordLove,
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Adriel,
                            characterSkin = AdminBRO.DialogCharacterSkin.Adriel,
                            message = "Ah, you're awake, Overlord! I guess it's been too long. Don't you remember me?",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right,
                            emotionAnimationId = (int)AnimationId.AdrielSurprised,
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Overlord,
                            characterSkin = AdminBRO.DialogCharacterSkin.Overlord,
                            message = "I have trouble remembering things. But you can give me a little reminder.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left,
                            emotionAnimationId = (int)AnimationId.OverlordIdle,
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Adriel,
                            characterSkin = AdminBRO.DialogCharacterSkin.Adriel,
                            message = "How's this for a reminder?",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right,
                            emotionAnimationId = (int)AnimationId.AdrielLove,
                            cutInAnimationId = (int)AnimationId.CutIn5,
                            cutInSoundPath = FMODEventPath.Dialogue3_CutInAdrielKiss
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            characterSkin = AdminBRO.DialogCharacterSkin.Ulvi,
                            message = "What. The fuck. Are you doing?!",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right,
                            emotionAnimationId = (int)AnimationId.UlviAngry,
                            cutInAnimationId = (int)AnimationId.CutIn5,
                            cutInSoundPath = FMODEventPath.Dialogue3_CutInAdrielKiss
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            characterSkin = AdminBRO.DialogCharacterSkin.UlviWolf,
                            message = "Listen, you big titty bitch. I can see that you have <b>a dick!</b> You're not fooling anyone with those melons.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right,
                            emotionAnimationId = (int)AnimationId.UlviWolfAngry,
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Overlord,
                            characterSkin = AdminBRO.DialogCharacterSkin.Overlord,
                            message = "Oh my!",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left,
                            emotionAnimationId = (int)AnimationId.OverlordSurprised,
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Adriel,
                            characterSkin = AdminBRO.DialogCharacterSkin.Adriel,
                            message = "I'm an angel, little puppy. I can have a dick or I can get rid of it if I so desire. Or if <b>anyone else</b> desires.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right,
                            emotionAnimationId = (int)AnimationId.AdrielIdle,
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Adriel,
                            characterSkin = AdminBRO.DialogCharacterSkin.Adriel,
                            message = "This is not why I'm here. If you ever want to get all your memories back, you better build <b>a Portal</b> and quickly. Then talk to me. Oh~ and bring your mangy dog. She's cute.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right,
                            emotionAnimationId = (int)AnimationId.AdrielLove,
                        },
                    }
                });

                //notifications
                //Battle tutorial 1
                notifications.Add(new AdminBRO.Dialog
                {
                    id = 1,
                    replicas = new List<AdminBRO.DialogReplica>()
                    {
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            characterSkin = AdminBRO.DialogCharacterSkin.Ulvi,
                            message =
                                "Let's see if you still have what it takes to kick some ass! A hundred years of sleep can make you a little rusty.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left,
                            emotionAnimationId = (int)AnimationId.UlviHappy,
                        }
                    }
                });

                //After battle 1
                notifications.Add(new AdminBRO.Dialog
                {
                    id = 2,
                    replicas = new List<AdminBRO.DialogReplica>()
                    {
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            characterSkin = AdminBRO.DialogCharacterSkin.Ulvi,
                            message =
                                "Whoa, you are good! But this was a pretty easy fight. Let's see how we can make you even stronger.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left,
                            emotionAnimationId = (int)AnimationId.UlviSurprised,
                        }
                    }
                });

                //Map tutorial
                notifications.Add(new AdminBRO.Dialog
                {
                    id = 3,
                    replicas = new List<AdminBRO.DialogReplica>()
                    {
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            characterSkin = AdminBRO.DialogCharacterSkin.Ulvi,
                            message =
                                "This is your map. Here you can <b>beat up</b> some cheeky fuckers or hang out with anyone you fancy and immerse yourself in their <b>story</b>. It’s all up to you!",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left,
                            emotionAnimationId = (int)AnimationId.UlviIdle,
                        }
                    }
                });

                //Battle tutorial 2
                notifications.Add(new AdminBRO.Dialog
                {
                    id = 4,
                    replicas = new List<AdminBRO.DialogReplica>()
                    {
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            characterSkin = AdminBRO.DialogCharacterSkin.Ulvi,
                            message =
                                "Alright, there are some enemies lurking about and looking for trouble. Let's get them!",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left,
                            emotionAnimationId = (int)AnimationId.UlviHappy,
                        }
                    }
                });

                //Buff tutorial after a failed battle
                notifications.Add(new AdminBRO.Dialog
                {
                    id = 5,
                    replicas = new List<AdminBRO.DialogReplica>()
                    {
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            characterSkin = AdminBRO.DialogCharacterSkin.Ulvi,
                            message =
                                "Huh… Looks like they are a bit tougher this time. Don't worry, and I have a solution! Let's get you a nifty boost. All you have to do is <b>fuck me.</b>",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left,
                            emotionAnimationId = (int)AnimationId.UlviSurprised,
                        }
                    }
                });

                //Post-buff dialogue
                notifications.Add(new AdminBRO.Dialog
                {
                    id = 6,
                    replicas = new List<AdminBRO.DialogReplica>()
                    {
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            characterSkin = AdminBRO.DialogCharacterSkin.Ulvi,
                            message =
                                "Did you like it as much as I did, Master? Enjoy your boost and come back for more!",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left,
                            emotionAnimationId = (int)AnimationId.UlviLove,
                        }
                    }
                });

                //Pre-battle dialogue (before getting the castle)
                notifications.Add(new AdminBRO.Dialog
                {
                    id = 7,
                    replicas = new List<AdminBRO.DialogReplica>
                    {
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            characterSkin = AdminBRO.DialogCharacterSkin.Ulvi,
                            message = "We’re almost at the Castle’s threshold! Let’s crack some skulls and make it our base.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left,
                            emotionAnimationId = (int)AnimationId.UlviHappy,
                        }
                    }
                });

                //Castle tutorial
                notifications.Add(new AdminBRO.Dialog
                {
                    id = 8,
                    replicas = new List<AdminBRO.DialogReplica>
                    {
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            characterSkin = AdminBRO.DialogCharacterSkin.Ulvi,
                            message = "This is your base, <b>the Castle</b>. It's not much to look at now, but we'll make it the stronghold you deserve!",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left,
                            emotionAnimationId = (int)AnimationId.UlviHappy,
                        }
                    }
                });

                //Ulvi thanking the Overlord for finishing her unique building
                notifications.Add(new AdminBRO.Dialog
                {
                    id = 9,
                    replicas = new List<AdminBRO.DialogReplica>
                    {
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            characterSkin = AdminBRO.DialogCharacterSkin.Ulvi,
                            message = "You've built this for me? That's so sweet! I don't even know how to thank you. Just kidding! Whip your dick out and I’ll get on my knees…",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left,
                            emotionAnimationId = (int)AnimationId.UlviHappy,
                        }
                    }
                });

                //Ulvi explains the purpose of memories
                notifications.Add(new AdminBRO.Dialog
                {
                    id = 10,
                    replicas = new List<AdminBRO.DialogReplica>
                    {
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            characterSkin = AdminBRO.DialogCharacterSkin.Ulvi,
                            message = "Looks like the shards you've collected from the Portal can be assembled into some dirty memories! Let’s check them out.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left,
                            emotionAnimationId = (int)AnimationId.UlviSurprised,
                        }
                    }
                });

                //Post-memory dialogue
                notifications.Add(new AdminBRO.Dialog
                {
                    id = 11,
                    replicas = new List<AdminBRO.DialogReplica>
                    {
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            characterSkin = AdminBRO.DialogCharacterSkin.Ulvi,
                            message = "That was pretty hot, huh? The more memories of others you watch, the faster you'll regain your own memories! Pretty sure my memories are the best — I savor <b>all</b> the dirty details.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left,
                            emotionAnimationId = (int)AnimationId.UlviHappy,
                        },
                    }
                });

                //animations
                //Overlord emotions
                animations.Add(new AdminBRO.Animation
                { 
                    id = (int)AnimationId.OverlordAngry,
                    title = "OverlordAngry",
                    layouts = new List<AdminBRO.AnimationData>
                    {
                        new AdminBRO.AnimationData
                        {
                            animationPath = "FTUE/Emotions/Overlord/angry_SkeletonData",
                            animationName = EmotionAnimationName.Angry
                        }
                    }
                });

                animations.Add(new AdminBRO.Animation
                {
                    id = (int)AnimationId.OverlordHappy,
                    title = "OverlordHappy",
                    layouts = new List<AdminBRO.AnimationData>
                    {
                        new AdminBRO.AnimationData
                        {
                            animationPath = "FTUE/Emotions/Overlord/happy_SkeletonData",
                            animationName = EmotionAnimationName.Happy
                        }
                    }
                });

                animations.Add(new AdminBRO.Animation
                {
                    id = (int)AnimationId.OverlordIdle,
                    title = "OverlordIdle",
                    layouts = new List<AdminBRO.AnimationData>
                    {
                        new AdminBRO.AnimationData
                        {
                            animationPath = "FTUE/Emotions/Overlord/idle_SkeletonData",
                            animationName = EmotionAnimationName.Idle
                        }
                    }
                });

                animations.Add(new AdminBRO.Animation
                {
                    id = (int)AnimationId.OverlordLove,
                    title = "OverlordLove",
                    layouts = new List<AdminBRO.AnimationData>
                    {
                        new AdminBRO.AnimationData
                        {
                            animationPath = "FTUE/Emotions/Overlord/love_SkeletonData",
                            animationName = EmotionAnimationName.Love
                        }
                    }
                });

                animations.Add(new AdminBRO.Animation
                {
                    id = (int)AnimationId.OverlordSurprised,
                    title = "OverlordSurprised",
                    layouts = new List<AdminBRO.AnimationData>
                    {
                        new AdminBRO.AnimationData
                        {
                            animationPath = "FTUE/Emotions/Overlord/surprised_SkeletonData",
                            animationName = EmotionAnimationName.Surprised
                        }
                    }
                });

                //Ulvi emotions
                animations.Add(new AdminBRO.Animation
                {
                    id = (int)AnimationId.UlviAngry,
                    title = "UlviAngry",
                    layouts = new List<AdminBRO.AnimationData>
                    {
                        new AdminBRO.AnimationData
                        {
                            animationPath = "FTUE/Emotions/Ulvi/angry_SkeletonData",
                            animationName = EmotionAnimationName.Angry
                        }
                    }
                });

                animations.Add(new AdminBRO.Animation
                {
                    id = (int)AnimationId.UlviHappy,
                    title = "UlviHappy",
                    layouts = new List<AdminBRO.AnimationData>
                    {
                        new AdminBRO.AnimationData
                        {
                            animationPath = "FTUE/Emotions/Ulvi/happy_SkeletonData",
                            animationName = EmotionAnimationName.Happy
                        }
                    }
                });

                animations.Add(new AdminBRO.Animation
                {
                    id = (int)AnimationId.UlviIdle,
                    title = "UlviIdle",
                    layouts = new List<AdminBRO.AnimationData>
                    {
                        new AdminBRO.AnimationData
                        {
                            animationPath = "FTUE/Emotions/Ulvi/idle_SkeletonData",
                            animationName = EmotionAnimationName.Idle
                        }
                    }
                });

                animations.Add(new AdminBRO.Animation
                {
                    id = (int)AnimationId.UlviLove,
                    title = "UlviLove",
                    layouts = new List<AdminBRO.AnimationData>
                    {
                        new AdminBRO.AnimationData
                        {
                            animationPath = "FTUE/Emotions/Ulvi/love_SkeletonData",
                            animationName = EmotionAnimationName.Love
                        }
                    }
                });

                animations.Add(new AdminBRO.Animation
                {
                    id = (int)AnimationId.UlviSurprised,
                    title = "UlviSurprised",
                    layouts = new List<AdminBRO.AnimationData>
                    {
                        new AdminBRO.AnimationData
                        {
                            animationPath = "FTUE/Emotions/Ulvi/surprised_SkeletonData",
                            animationName = EmotionAnimationName.Surprised
                        }
                    }
                });

                //UlviWolf emotions
                animations.Add(new AdminBRO.Animation
                {
                    id = (int)AnimationId.UlviWolfAngry,
                    title = "UlviWolfAngry",
                    layouts = new List<AdminBRO.AnimationData>
                    {
                        new AdminBRO.AnimationData
                        {
                            animationPath = "FTUE/Emotions/UlviFurry/angry_SkeletonData",
                            animationName = EmotionAnimationName.Angry
                        }
                    }
                });

                animations.Add(new AdminBRO.Animation
                {
                    id = (int)AnimationId.UlviWolfHappy,
                    title = "UlviWolfHappy",
                    layouts = new List<AdminBRO.AnimationData>
                    {
                        new AdminBRO.AnimationData
                        {
                            animationPath = "FTUE/Emotions/UlviFurry/happy_SkeletonData",
                            animationName = EmotionAnimationName.Happy
                        }
                    }
                });

                animations.Add(new AdminBRO.Animation
                {
                    id = (int)AnimationId.UlviWolfIdle,
                    title = "UlviWolfIdle",
                    layouts = new List<AdminBRO.AnimationData>
                    {
                        new AdminBRO.AnimationData
                        {
                            animationPath = "FTUE/Emotions/UlviFurry/idle_SkeletonData",
                            animationName = EmotionAnimationName.Idle
                        }
                    }
                });

                animations.Add(new AdminBRO.Animation
                {
                    id = (int)AnimationId.UlviWolfLove,
                    title = "UlviWolfLove",
                    layouts = new List<AdminBRO.AnimationData>
                    {
                        new AdminBRO.AnimationData
                        {
                            animationPath = "FTUE/Emotions/UlviFurry/love_SkeletonData",
                            animationName = EmotionAnimationName.Love
                        }
                    }
                });

                animations.Add(new AdminBRO.Animation
                {
                    id = (int)AnimationId.UlviWolfSurprised,
                    title = "UlviWolfSurprised",
                    layouts = new List<AdminBRO.AnimationData>
                    {
                        new AdminBRO.AnimationData
                        {
                            animationPath = "FTUE/Emotions/UlviFurry/surprised_SkeletonData",
                            animationName = EmotionAnimationName.Surprised
                        }
                    }
                });

                //Adriel emotions
                animations.Add(new AdminBRO.Animation
                {
                    id = (int)AnimationId.AdrielAngry,
                    title = "AdrielAngry",
                    layouts = new List<AdminBRO.AnimationData>
                    {
                        new AdminBRO.AnimationData
                        {
                            animationPath = "FTUE/Emotions/Adriel/angry_SkeletonData",
                            animationName = EmotionAnimationName.Angry
                        }
                    }
                });

                animations.Add(new AdminBRO.Animation
                {
                    id = (int)AnimationId.AdrielHappy,
                    title = "AdrielHappy",
                    layouts = new List<AdminBRO.AnimationData>
                    {
                        new AdminBRO.AnimationData
                        {
                            animationPath = "FTUE/Emotions/Adriel/happy_SkeletonData",
                            animationName = EmotionAnimationName.Happy
                        }
                    }
                });

                animations.Add(new AdminBRO.Animation
                {
                    id = (int)AnimationId.AdrielIdle,
                    title = "AdrielIdle",
                    layouts = new List<AdminBRO.AnimationData>
                    {
                        new AdminBRO.AnimationData
                        {
                            animationPath = "FTUE/Emotions/Adriel/idle_SkeletonData",
                            animationName = EmotionAnimationName.Idle
                        }
                    }
                });

                animations.Add(new AdminBRO.Animation
                {
                    id = (int)AnimationId.AdrielLove,
                    title = "AdrielLove",
                    layouts = new List<AdminBRO.AnimationData>
                    {
                        new AdminBRO.AnimationData
                        {
                            animationPath = "FTUE/Emotions/Adriel/love_SkeletonData",
                            animationName = EmotionAnimationName.Love
                        }
                    }
                });

                animations.Add(new AdminBRO.Animation
                {
                    id = (int)AnimationId.AdrielSurprised,
                    title = "AdrielSurprised",
                    layouts = new List<AdminBRO.AnimationData>
                    {
                        new AdminBRO.AnimationData
                        {
                            animationPath = "FTUE/Emotions/Adriel/surprised_SkeletonData",
                            animationName = EmotionAnimationName.Surprised
                        }
                    }
                });

                //cutIn-s
                animations.Add(new AdminBRO.Animation
                {
                    id = (int)AnimationId.CutIn1,
                    title = "CutIn1",
                    layouts = new List<AdminBRO.AnimationData>
                    {
                        new AdminBRO.AnimationData
                        {
                            animationPath = "FTUE/CutInAnims/CutIn1/back_SkeletonData",
                            animationName = "back"
                        },
                        new AdminBRO.AnimationData
                        {
                            animationPath = "FTUE/CutInAnims/CutIn1/idle01_SkeletonData",
                            animationName = "idle"
                        }
                    }
                });

                animations.Add(new AdminBRO.Animation
                {
                    id = (int)AnimationId.CutIn2,
                    title = "CutIn2",
                    layouts = new List<AdminBRO.AnimationData>
                    {
                        new AdminBRO.AnimationData
                        {
                            animationPath = "FTUE/CutInAnims/CutIn2/back_SkeletonData",
                            animationName = "back"
                        },
                        new AdminBRO.AnimationData
                        {
                            animationPath = "FTUE/CutInAnims/CutIn2/idle01_SkeletonData",
                            animationName = "idle"
                        }
                    }
                });

                animations.Add(new AdminBRO.Animation
                {
                    id = (int)AnimationId.CutIn3,
                    title = "CutIn3",
                    layouts = new List<AdminBRO.AnimationData>
                    {
                        new AdminBRO.AnimationData
                        {
                            animationPath = "FTUE/CutInAnims/CutIn3/back_SkeletonData",
                            animationName = "back"
                        },
                        new AdminBRO.AnimationData
                        {
                            animationPath = "FTUE/CutInAnims/CutIn3/idle_SkeletonData",
                            animationName = "idle"
                        }
                    }
                });

                animations.Add(new AdminBRO.Animation
                {
                    id = (int)AnimationId.CutIn4,
                    title = "CutIn4",
                    layouts = new List<AdminBRO.AnimationData>
                    {
                        new AdminBRO.AnimationData
                        {
                            animationPath = "FTUE/CutInAnims/CutIn4/back_overlord_SkeletonData",
                            animationName = "back"
                        }
                    }
                });

                animations.Add(new AdminBRO.Animation
                {
                    id = (int)AnimationId.CutIn5,
                    title = "CutIn5",
                    layouts = new List<AdminBRO.AnimationData>
                    {
                        new AdminBRO.AnimationData
                        {
                            animationPath = "FTUE/CutInAnims/CutIn5/back_SkeletonData",
                            animationName = "back"
                        },
                        new AdminBRO.AnimationData
                        {
                            animationPath = "FTUE/CutInAnims/CutIn5/idle01_SkeletonData",
                            animationName = "idle"
                        }
                    }
                });

                animations.Add(new AdminBRO.Animation
                {
                    id = (int)AnimationId.CutIn6,
                    title = "CutIn6",
                    layouts = new List<AdminBRO.AnimationData>
                    {
                        new AdminBRO.AnimationData
                        {
                            animationPath = "FTUE/CutInAnims/CutIn6/back_SkeletonData",
                            animationName = "back"
                        },
                        new AdminBRO.AnimationData
                        {
                            animationPath = "FTUE/CutInAnims/CutIn6/idle01_SkeletonData",
                            animationName = "idle"
                        }
                    }
                });

                animations.Add(new AdminBRO.Animation
                {
                    id = (int)AnimationId.CutIn7,
                    title = "CutIn7",
                    layouts = new List<AdminBRO.AnimationData>
                    {
                        new AdminBRO.AnimationData
                        {
                            animationPath = "FTUE/CutInAnims/CutIn7/back_SkeletonData",
                            animationName = "back"
                        },
                        new AdminBRO.AnimationData
                        {
                            animationPath = "FTUE/CutInAnims/CutIn7/idle01_SkeletonData",
                            animationName = "idle"
                        }
                    }
                });

                animations.Add(new AdminBRO.Animation
                {
                    id = (int)AnimationId.CutIn8,
                    title = "CutIn8",
                    layouts = new List<AdminBRO.AnimationData>
                    {
                        new AdminBRO.AnimationData
                        {
                            animationPath = "FTUE/CutInAnims/CutIn8/back_SkeletonData",
                            animationName = "back"
                        },
                        new AdminBRO.AnimationData
                        {
                            animationPath = "FTUE/CutInAnims/CutIn8/idle01_SkeletonData",
                            animationName = "idle"
                        }
                    }
                });

                animations.Add(new AdminBRO.Animation
                {
                    id = (int)AnimationId.CutIn9,
                    title = "CutIn9",
                    layouts = new List<AdminBRO.AnimationData>
                    {
                        new AdminBRO.AnimationData
                        {
                            animationPath = "FTUE/CutInAnims/CutIn9/back_SkeletonData",
                            animationName = "back"
                        },
                        new AdminBRO.AnimationData
                        {
                            animationPath = "FTUE/CutInAnims/CutIn9/idle01_SkeletonData",
                            animationName = "idle"
                        }
                    }
                });

                //main-s
                animations.Add(new AdminBRO.Animation
                {
                    id = (int)AnimationId.MainSex1,
                    title = "MainSex1",
                    layouts = new List<AdminBRO.AnimationData>
                    {
                        new AdminBRO.AnimationData
                        {
                            animationPath = "FTUE/MainSexAnims/MainSex1/back_SkeletonData",
                            animationName = "back"
                        },
                        new AdminBRO.AnimationData
                        {
                            animationPath = "FTUE/MainSexAnims/MainSex1/idle01_SkeletonData",
                            animationName = "idle"
                        }
                    }
                });

                animations.Add(new AdminBRO.Animation
                {
                    id = (int)AnimationId.FinalSex1,
                    title = "FinalSex1",
                    layouts = new List<AdminBRO.AnimationData>
                    {
                        new AdminBRO.AnimationData
                        {
                            animationPath = "FTUE/MainSexAnims/FinalSex1/back_SkeletonData",
                            animationName = "back"
                        },
                        new AdminBRO.AnimationData
                        {
                            animationPath = "FTUE/MainSexAnims/FinalSex1/idle01_SkeletonData",
                            animationName = "idle"
                        }
                    }
                });

                animations.Add(new AdminBRO.Animation
                {
                    id = (int)AnimationId.MainSex2,
                    title = "MainSex2",
                    layouts = new List<AdminBRO.AnimationData>
                    {
                        new AdminBRO.AnimationData
                        {
                            animationPath = "FTUE/MainSexAnims/MainSex2/back_SkeletonData",
                            animationName = "back"
                        },
                        new AdminBRO.AnimationData
                        {
                            animationPath = "FTUE/MainSexAnims/MainSex2/idle01_SkeletonData",
                            animationName = "idle"
                        }
                    }
                });

                animations.Add(new AdminBRO.Animation
                {
                    id = (int)AnimationId.FinalSex2,
                    title = "FinalSex2",
                    layouts = new List<AdminBRO.AnimationData>
                    {
                        new AdminBRO.AnimationData
                        {
                            animationPath = "FTUE/MainSexAnims/FinalSex2/back_SkeletonData",
                            animationName = "back"
                        },
                        new AdminBRO.AnimationData
                        {
                            animationPath = "FTUE/MainSexAnims/FinalSex2/idle01_SkeletonData",
                            animationName = "idle"
                        }
                    }
                });

                animations.Add(new AdminBRO.Animation
                {
                    id = (int)AnimationId.MainSex3,
                    title = "MainSex3",
                    layouts = new List<AdminBRO.AnimationData>
                    {
                        new AdminBRO.AnimationData
                        {
                            animationPath = "FTUE/MainSexAnims/MainSex3/back_SkeletonData",
                            animationName = "back"
                        },
                        new AdminBRO.AnimationData
                        {
                            animationPath = "FTUE/MainSexAnims/MainSex3/idle01_SkeletonData",
                            animationName = "idle"
                        }
                    }
                });

                animations.Add(new AdminBRO.Animation
                {
                    id = (int)AnimationId.FinalSex3,
                    title = "FinalSex3",
                    layouts = new List<AdminBRO.AnimationData>
                    {
                        new AdminBRO.AnimationData
                        {
                            animationPath = "FTUE/MainSexAnims/FinalSex3/back_SkeletonData",
                            animationName = "back"
                        },
                        new AdminBRO.AnimationData
                        {
                            animationPath = "FTUE/MainSexAnims/FinalSex3/idle01_SkeletonData",
                            animationName = "idle"
                        }
                    }
                });
            }
        }
    }
}
