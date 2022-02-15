using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Overlewd
{
    namespace FTUE
    {

        public static class GameData
        {
            public static AdminBRO.Dialog GetSexById(int id)
            {
                return sexs.Find(d => d.id == id);
            }
            public static List<AdminBRO.Dialog> sexs = new List<AdminBRO.Dialog>();

            public static AdminBRO.Dialog GetDialogById(int id) 
            {
                return dialogs.Find(d => d.id == id);
            }
            public static List<AdminBRO.Dialog> dialogs = new List<AdminBRO.Dialog>();

            public static AdminBRO.Dialog GetNotificationById(int id)
            {
                return notifications.Find(d => d.id == id);
            }
            public static List<AdminBRO.Dialog> notifications = new List<AdminBRO.Dialog>();

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
                            animation = AdminBRO.DialogCharacterAnimation.Surprised,
                            message = "I can't believe I found him! But… How do I wake him up?"
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            message = "Well, at least one part of him seems to be up. I can work with that!",
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Happy,
                            replicaMainSoundPath = SoundManager.SoundPath.Animations.MainScene,
                            message = "You're finally awake! Took you long enough, Master.",
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            replicaMainSoundPath = SoundManager.SoundPath.Animations.MainScene,
                            message = "M-m-m, your cock tastes so good... I guess the legends were true."
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            replicaCutInSoundPath = SoundManager.SoundPath.Animations.CutInLick,
                            message = "I can lick your cock for hours. It feels so smooth and silky...",
                            cutIn = "CutIn1"
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            replicaCutInSoundPath = SoundManager.SoundPath.Animations.CutInLick,
                            message = "You're so big. I can barely fit you inside my tight mouth.",
                            cutIn = "CutIn1"
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            replicaMainSoundPath = SoundManager.SoundPath.Animations.MainScene,
                            message = "Please, cum for me, Master! I want to taste you so bad…"
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Happy,
                            replicaCutInSoundPath = SoundManager.SoundPath.Animations.CutInCumshot,
                            message = "Thank you, Master. I'm so happy!",
                            cutIn = "CutIn2"
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            replicaMainSoundPath = SoundManager.SoundPath.Animations.CutInCumshot,
                            message = "You taste exactly how I imagined. After all those years searching for you…",
                            cutIn = "CutIn2"
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Happy,
                            message = "Now put your pants on, and let's get cracking! We have a realm to conquer."
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
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            message = "Master, watching you annihilate your foes has made me so horny…"
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            message = "Ah~ You pulled my tail out! I'm so embarrassed… But it feels so good!",
                            cutIn = "CutIn8"
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            message =
                                "I've been preparing all my holes for you to fill. I think I'm addicted to your cock.",
                            cutIn = "CutIn8"
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            message = "Ummh~ I want to take your hot load… Cum with me, Master!",
                            cutIn = "CutIn8"
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            message = "Aahh~ Aahh! Just like that!",
                            cutIn = "CutIn9"
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            message = "Mmm~ Fuck yes!",
                            cutIn = "CutIn9"
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            message = "Your load feels so hot on my pussy… Thank you for treating me, Master.",
                            cutIn = "CutIn9"
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            message =
                                "Please, don't forget to play with me often. All of my holes are ready to serve you whenever you want!"
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
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            message = "Your nimble fingers playing with my pussy make me so horny…"
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            message = "Is this what the touch of a Master feels like?",
                            cutIn = "CutIn6"
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            message = "My clit is throbbing under your touch, it feels so good!",
                            cutIn = "CutIn6"
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            message = "Ahh~ I wish you'd fuck me already and make me yours forever.",
                            cutIn = "CutIn6"
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            message = "Umm~ I've creamed all over your fingers, oops.",
                            cutIn = "CutIn7"
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            message = "Now your cock will slide in so easily… And you can be my Master.",
                            cutIn = "CutIn7"
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Surprised,
                            message = "What do you mean you're not ready? I was ready! I was a good girl! I… Wasn't I?",
                            cutIn = "CutIn7"
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Angry,
                            message = "Fine! Walk away. No — fuck off forever! I know how my Master will feel like. He'd fuck me so much better!"
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
                            animation = AdminBRO.DialogCharacterAnimation.Happy,
                            message =
                                "I'm so happy I found you, Master! Wolves without a master are like humans without arms.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Overlord,
                            animation = AdminBRO.DialogCharacterAnimation.Idle,
                            message = "So why did you pick <b>me?</b>",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Happy,
                            message =
                                "Ah… All those stories about you fascinated me. I want to help you retake what's yours!",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Overlord,
                            animation = AdminBRO.DialogCharacterAnimation.Idle,
                            message = "You're a good girl, Ulvi.",
                            cutIn = "CutIn3",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Happy,
                            message = "A hundred years of searching for you has been worth it.",
                            cutIn = "CutIn3",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Overlord,
                            animation = AdminBRO.DialogCharacterAnimation.Idle,
                            message =
                                "A hundred years? Sheesh. I don't even remember anything that happened before I fell asleep.",
                            cutIn = "CutIn3",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Idle,
                            message =
                                "Oh, it's okay, Master. We'll get your memories back! Magical slumber tends to have an amnesia-like effect.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right
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
                            animation = AdminBRO.DialogCharacterAnimation.Surprised,
                            message = "You really are the guy from all those legends I heard my whole life! Strong, determined, bloodthirsty.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Overlord,
                            animation = AdminBRO.DialogCharacterAnimation.Idle,
                            message = "Are there legends about me? <b>Sweet.</b> Tell me more.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Happy,
                            message = "Oh, you were <b>the Big Bad!</b> When you approached with your armies, children cried, soldiers shat themselves. It was great! The Empress herself was involved. She became your main rival.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Happy,
                            message = "Man, she hated you so much! And what she hated the most was that she was so horny for you!",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Overlord,
                            animation = AdminBRO.DialogCharacterAnimation.Idle,
                            message = "Horny? Huh. Before we get sidetracked — why was I even asleep when you found me?",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Surprised,
                            message = "Uhh… The thing is… You really wanna know?",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Overlord,
                            animation = AdminBRO.DialogCharacterAnimation.Angry,
                            message = "You have to tell me. Or I'll have to punish you.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            message = "Ah, you know I'm <b>a sucker</b> for punishment! Okay, so that's what I heard...",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Idle,
                            message = "You just raided one of the rival camps and were coming back to your stronghold with all the heavy equipment. So the old bridge across the moat broke under your weight.",
                            cutIn = "CutIn4",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Idle,
                            message = "Your camp was destroyed and you were captured. Unconcious and covered in sludge. The Empress' people brushed you off a bit and transported to the castle. I wish it was more flattering... ",
                            cutIn = "CutIn4",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Overlord,
                            animation = AdminBRO.DialogCharacterAnimation.Idle,
                            message = "At least that explains the fishy smell.",
                            cutIn = "CutIn4",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left
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
                            animation = AdminBRO.DialogCharacterAnimation.Happy,
                            message = "Master, I think we're ready to expand. This location would be <b>perfect</b> for a new camp!",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Adriel,
                            animation = AdminBRO.DialogCharacterAnimation.Angry,
                            message = "This location is in my domain, dog. I'd rather it remain campless. What do we have here?",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Overlord,
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            message = "Whoa. Hey there, beautiful.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Adriel,
                            animation = AdminBRO.DialogCharacterAnimation.Surprised,
                            message = "Ah, you're awake, Overlord! I guess it's been too long. Don't you remember me?",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Overlord,
                            animation = AdminBRO.DialogCharacterAnimation.Idle,
                            message = "I have trouble remembering things. But you can give me a little reminder.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Adriel,
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            message = "How's this for a reminder?",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right,
                            cutIn = "CutIn5"
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Angry,
                            message = "What. The fuck. Are you doing?!",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right,
                            cutIn = "CutIn5"
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            characterSkin = AdminBRO.DialogCharacterSkin.Wolf,
                            animation = AdminBRO.DialogCharacterAnimation.Angry,
                            message = "Listen, you big titty bitch. I can see that you have <b>a dick!</b> You're not fooling anyone with those melons.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Overlord,
                            animation = AdminBRO.DialogCharacterAnimation.Surprised,
                            message = "Oh my!",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Adriel,
                            animation = AdminBRO.DialogCharacterAnimation.Idle,
                            message = "I'm an angel, little puppy. I can have a dick or I can get rid of it if I so desire. Or if <b>anyone else</b> desires.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Adriel,
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            message = "This is not why I'm here. If you ever want to get all your memories back, you better build <b>a Portal</b> and quickly. Then talk to me. Oh~ and bring your mangy dog. She's cute.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right
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
                            animation = AdminBRO.DialogCharacterAnimation.Happy,
                            message =
                                "Let's see if you still have what it takes to kick some ass! A hundred years of sleep can make you a little rusty.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left
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
                            animation = AdminBRO.DialogCharacterAnimation.Surprised,
                            message =
                                "Whoa, you are good! But this was a pretty easy fight. Let's see how we can make you even stronger.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left
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
                            animation = AdminBRO.DialogCharacterAnimation.Idle,
                            message =
                                "This is your map. Here you can <b>beat up</b> some cheeky fuckers or hang out with anyone you fancy and immerse yourself in their <b>story</b>. It’s all up to you!",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left
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
                            animation = AdminBRO.DialogCharacterAnimation.Happy,
                            message =
                                "Alright, there are some enemies lurking about and looking for trouble. Let's get them!",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left
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
                            animation = AdminBRO.DialogCharacterAnimation.Surprised,
                            message =
                                "Huh… Looks like they are a bit tougher this time. Don't worry, and I have a solution! Let's get you a nifty boost. All you have to do is <b>fuck me.</b>",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left
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
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            message =
                                "Did you like it as much as I did, Master? Enjoy your boost and come back for more!",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left
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
                            animation = AdminBRO.DialogCharacterAnimation.Happy,
                            message = "We’re almost at the Castle’s threshold! Let’s crack some skulls and make it our base.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left
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
                            animation = AdminBRO.DialogCharacterAnimation.Happy,
                            message = "This is your base, <b>the Castle</b>. It's not much to look at now, but we'll make it the stronghold you deserve!",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left
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
                            animation = AdminBRO.DialogCharacterAnimation.Happy,
                            message = "You've built this for me? That's so sweet! I don't even know how to thank you. Just kidding! Whip your dick out and I’ll get on my knees…",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left
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
                            animation = AdminBRO.DialogCharacterAnimation.Surprised,
                            message = "Looks like the shards you've collected from the Portal can be assembled into some dirty memories! Let’s check them out.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left
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
                            animation = AdminBRO.DialogCharacterAnimation.Happy,
                            message = "That was pretty hot, huh? The more memories of others you watch, the faster you'll regain your own memories! Pretty sure my memories are the best — I savor <b>all</b> the dirty details.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left
                        },
                    }
                });
            }
        }
    }
}
