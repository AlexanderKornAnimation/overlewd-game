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
            public static AdminBRO.Dialog GetDialogById(int id) 
            {
                return dialogs.Find(d => d.id == id);
            }
            public static List<AdminBRO.Dialog> dialogs = new List<AdminBRO.Dialog>();

            public static void Initialization()
            {
                //test dialog 0
                dialogs.Add(new AdminBRO.Dialog
                {
                    id = 0,
                    replicas = new List<AdminBRO.DialogReplica>
                    {
                        new AdminBRO.DialogReplica {characterName = "PersName1", message = "Message 1"},
                        new AdminBRO.DialogReplica {characterName = "PersName2", message = "Message 2"},
                        new AdminBRO.DialogReplica {characterName = "PersName1", message = "Message 3"},
                        new AdminBRO.DialogReplica {characterName = "PersName2", message = "Message 4"},
                    }
                });

                //test dialog 1
                dialogs.Add(new AdminBRO.Dialog
                {
                    id = 1,
                    replicas = new List<AdminBRO.DialogReplica>
                    {
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Overlord,
                            message = "Message Owerlord",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            message = "Message Ulvi",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Faye,
                            message = "Message Faye",
                            characterPosition = AdminBRO.DialogCharacterPosition.Middle
                        },
                    }
                });

                //Intro/sex scene SEX 1 STORY 1
                dialogs.Add(new AdminBRO.Dialog
                {
                    id = 2,
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
                            message = "You're finally awake! Took you long enough, Master.",
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            message = "M-m-m, your cock tastes so good... I guess the legends were true."
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            message = "I can lick your cock for hours. It feels so smooth and silky...",
                            cutIn = "Cut-in 1: licking"
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            message = "You're so big. I can barely fit you inside my tight mouth.",
                            cutIn = "Cut-in 1: licking"
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            message = "Thank you, Master. I'm so happy!"
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Surprised,
                            message = "You taste exactly how I imagined. After all those years searching for you…",
                            cutIn = "Cut-in 2: cumshot"
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Surprised,
                            message = "Now put your pants on, and let's get cracking! We have a realm to conquer."
                        },
                    }
                });

                //Battle tutorial 1
                dialogs.Add(new AdminBRO.Dialog
                {
                    id = 3,
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
                dialogs.Add(new AdminBRO.Dialog
                {
                    id = 4,
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
                dialogs.Add(new AdminBRO.Dialog
                {
                    id = 5,
                    replicas = new List<AdminBRO.DialogReplica>()
                    {
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Idle,
                            message =
                                "This is your map. Here you can beat up some cheeky fuckers or hang out with anyone you fancy and immerse yourself in their story. It’s all up to you!",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left
                        }
                    }
                });

                //Battle tutorial 2
                dialogs.Add(new AdminBRO.Dialog
                {
                    id = 6,
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
                dialogs.Add(new AdminBRO.Dialog
                {
                    id = 7,
                    replicas = new List<AdminBRO.DialogReplica>()
                    {
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Surprised,
                            message =
                                "Huh… Looks like they are a bit tougher this time. Don't worry, and I have a solution! Let's get you a nifty boost. All you have to do is fuck me.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left
                        }
                    }
                });

                //Buff sex scene SEX 2
                dialogs.Add(new AdminBRO.Dialog
                {
                    id = 8,
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
                            cutIn = "Cut-in 8: taking the tail out"
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            message =
                                "I've been preparing all my holes for you to fill. I think I'm addicted to your cock.",
                            cutIn = "Cut-in 8: taking the tail out"
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            message = "Ummh~ I want to take your hot load… Cum with me, Master!",
                            cutIn = "Cut-in 8: taking the tail out"
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            message = "Aahh~ Aahh! Just like that!",
                            cutIn = "Cut-in 9: creampie"
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            message = "Mmm~ Fuck yes!",
                            cutIn = "Cut-in 9: creampie"
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            message = "Your load feels so hot on my pussy… Thank you for treating me, Master.",
                            cutIn = "Cut-in 9: creampie"
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

                //Post-buff dialogue
                dialogs.Add(new AdminBRO.Dialog
                {
                    id = 9,
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

                //Story dialogue STORY 2
                dialogs.Add(new AdminBRO.Dialog
                {
                    id = 10,
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
                            message = "So why did you pick me?",
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
                            cutIn = "Cut-in 3: Ulvi scratch",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Happy,
                            message = "A hundred years of searching for you has been worth it.",
                            cutIn = "Cut-in 3: Ulvi scratch",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Overlord,
                            animation = AdminBRO.DialogCharacterAnimation.Idle,
                            message =
                                "A hundred years? Sheesh. I don't even remember anything that happened before I fell asleep.",
                            cutIn = "Cut-in 3: Ulvi scratch",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left
                        },
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Idle,
                            message =
                                "Oh, it's okay, Master. Magical slumber tends to have an amnesia-like effect. We�ll get your memories back!",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right
                        },
                    }
                });

                //Pre-battle dialogue (before getting the castle)
                dialogs.Add(new AdminBRO.Dialog
                {
                    id = 11,
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
                dialogs.Add(new AdminBRO.Dialog
                {
                    id = 12,
                    replicas = new List<AdminBRO.DialogReplica>
                    {
                        new AdminBRO.DialogReplica
                        {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Happy,
                            message = "This is your base, the Castle. It's not much to look at now, but we'll make it the stronghold you deserve!",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left
                        }
                    }
                });
                
                //Story dialogue STORY 3
                dialogs.Add(new AdminBRO.Dialog
                {
                    id = 13,
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
                            message = "Are there legends about me? Sweet. Tell me more.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Happy,
                            message = "Oh, you were the Big Bad! When you approached with your armies, children cried, soldiers shat themselves. It was great! The Empress herself was involved. She became your main rival.",
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
                            message = "Ah, you know I'm a sucker for punishment! Okay, so that's what I heard...",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Idle,
                            message = "You just raided one of the rival camps and were coming back to your stronghold with all the heavy equipment. So the old bridge across the moat broke under your weight.",
                            cutIn = "Cut-in 4: Overlord's drowning",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Idle,
                            message = "Your camp was destroyed and you were captured. Unconcious and covered in sludge. The Empress' people brushed you off a bit and transported to the castle. I wish it was more flattering... ",
                            cutIn = "Cut-in 4: Overlord's drowning",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Overlord,
                            animation = AdminBRO.DialogCharacterAnimation.Idle,
                            message = "At least that explains the fishy smell.",
                            cutIn = "Cut-in 4: Overlord's drowning",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left
                        },
                    }
                });

                //Story dialogue regarding the portal STORY 4
                dialogs.Add(new AdminBRO.Dialog
                {
                    id = 14,
                    replicas = new List<AdminBRO.DialogReplica>
                    {
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Happy,
                            message = "Master, I think we�re ready to expand. This location would be perfect for a new camp!",
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
                            cutIn = "Cut-in 5: Adriel kissing Overlord's helmet"
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Angry,
                            message = "What. The fuck. Are you doing?!",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right,
                            cutIn = "Cut-in 5: Adriel kissing Overlord's helmet"
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.UlviWolf,
                            animation = AdminBRO.DialogCharacterAnimation.Angry,
                            message = "Listen, you big titty bitch. You're not fooling anyone with those melons. I can see that you have a dick.",
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
                            message = "I'm an angel, little puppy. I can have a dick or I can get rid of it if I so desire. Or if anyone else desires.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Adriel,
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            message = "This is not why I'm here. Heed my warning, Overlord. I will not surrender to you easily. If you want to defeat me, you'd have to work very, very hard.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right
                        },
                    }
                });

                //Ulvi explains the purpose of memories
                dialogs.Add(new AdminBRO.Dialog
                {
                    id = 15,
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
                
                //Memory sex scene SEX 3
                dialogs.Add(new AdminBRO.Dialog
                {
                    id = 16,
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
                            cutIn = "Cut-in 6: pussy rubbing"
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            message = "My clit is throbbing under your touch, it feels so good!",
                            cutIn = "Cut-in 6: pussy rubbing"
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            message = "Ahh~ I wish you'd fuck me already and make me yours forever.",
                            cutIn = "Cut-in 6: pussy rubbing"
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            message = "Umm~ I've creamed all over your fingers, oops.",
                            cutIn = "Cut-in 7: Ulvi's orgasm"
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            message = "Now your cock will slide in so easily… And you can be my Master.",
                            cutIn = "Cut-in 7: Ulvi's orgasm"
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Surprised,
                            message = "What do you mean you're not ready? I was ready! I was a good girl! I… Wasn't I?",
                            cutIn = "Cut-in 7: Ulvi's orgasm"
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Angry,
                            message = "Fine! Walk away. No — fuck off forever! I know how my Master will feel like. He'd fuck me so much better!"
                        },
                    }
                });

                //Post-memory dialogue
                dialogs.Add(new AdminBRO.Dialog
                {
                    id = 17,
                    replicas = new List<AdminBRO.DialogReplica>
                    {
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Happy,
                            message = "That was pretty hot, huh? The more memories of others you watch, the faster you'll regain your own memories! Pretty sure my memories are the best — I savor all the dirty details.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left
                        },
                    }
                });
            }
        }
    }
}
