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
                dialogs.Add(new AdminBRO.Dialog {
                    id = 0,
                    replicas = new List<AdminBRO.DialogReplica>
                    {
                        new AdminBRO.DialogReplica { characterName = "PersName1", message = "Message 1" },
                        new AdminBRO.DialogReplica { characterName = "PersName2", message = "Message 2" },
                        new AdminBRO.DialogReplica { characterName = "PersName1", message = "Message 3" },
                        new AdminBRO.DialogReplica { characterName = "PersName2", message = "Message 4" },
                    }
                });

                //test dialog 1
                dialogs.Add(new AdminBRO.Dialog
                {
                    id = 1,
                    replicas = new List<AdminBRO.DialogReplica>
                    {
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Overlord, 
                            message = "Message Owerlord",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            message = "Message Ulvi",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Faye,
                            message = "Message Faye",
                            characterPosition = AdminBRO.DialogCharacterPosition.Middle
                        },
                    }
                });

                //Intro Sex1
                dialogs.Add(new AdminBRO.Dialog
                {
                    id = 2,
                    replicas = new List<AdminBRO.DialogReplica>
                    {
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Happy,
                            message = "You’re finally awake! Took you long enough, Master."
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Happy,
                            message = "Thank you, Master. I’m so happy!",
                            cutIn = "Cut-in 2: cumshot"
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            message = "Well, at least one part of him seems to be up. I can work with that!"
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            message = "M-m-m, your cock tastes so good… I guess the legends were true."
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            message = "I can lick your cock for hours. It feels so smooth and silky…",
                            cutIn = "Cut-in 1: licking"
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            message = "You’re so big. I can barely fit you inside my tight mouth."
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            message = "Please, cum for me, Master! I want to taste you so bad…"
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Surprised,
                            message = "I can’t believe I found him! But… How do I wake him up?"
                        },
                    }
                });

                //Dialog1
                dialogs.Add(new AdminBRO.Dialog
                {
                    id = 3,
                    replicas = new List<AdminBRO.DialogReplica>
                    {
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Happy,
                            message = "I'm so happy I found you, Master! Wolves without a master are like humans without arms.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Overlord,
                            animation = AdminBRO.DialogCharacterAnimation.Idle,
                            message = "What is your name?",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Happy,
                            message = "Ulvi, Master! It’s my pleasure to serve you. And please you.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Overlord,
                            animation = AdminBRO.DialogCharacterAnimation.Idle,
                            message = "You’re a good girl, Ulvi.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Happy,
                            message = "A hundred years of searching for you has been worth it.",
                            cutIn = "Cut-in 3: Ulvi scratch",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Overlord,
                            animation = AdminBRO.DialogCharacterAnimation.Idle,
                            message = "A hundred years? Sheesh. I don't even remember anything that happened before I fell asleep.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Idle,
                            message = "Oh, it’s okay, Master. Magical slumber tends to have an amnesia-like effect. We’ll get your memories back!",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right
                        },
                    }
                });

                //Dialog2
                dialogs.Add(new AdminBRO.Dialog
                {
                    id = 4,
                    replicas = new List<AdminBRO.DialogReplica>
                    {
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Surprised,
                            message = "Wow, you really are the guy from all those legends I heard my whole life! Strong, determined, bloodthirsty.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Overlord,
                            animation = AdminBRO.DialogCharacterAnimation.Idle,
                            message = "There are legends about me? Sweet. Tell me more.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Happy,
                            message = "Oh, you were the Big Bad! Everyone was terrified when you approached their domain with your armies.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Happy,
                            message = "Children cried. Soldiers would shit themselves. It was great! The Empress herself was involved, she was your main rival.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Overlord,
                            animation = AdminBRO.DialogCharacterAnimation.Idle,
                            message = "So why was I asleep when you found me?",
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
                            message = "You have to tell me. Or I’ll have to punish you.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            message = "Ah, you know I'm a sucker for punishment! Okay, so that's what I heard…",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Idle,
                            message = "You just raided one of the rival camps and were coming back to your stronghold with all the heavy equipment. So the old bridge across the moat broke under your weight.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Idle,
                            message = "Your camp was destroyed and you were captured. Unconcious and covered in sludge. The Empress' people brushed you off a bit and transported to the castle. I wish it was more flattering… ",
                            cutIn = "Cut-in 4: Overlord’s drowning",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Overlord,
                            animation = AdminBRO.DialogCharacterAnimation.Idle,
                            message = "At least that explains the fishy smell.",
                            cutIn = "Cut-in 4: Overlord’s drowning",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left
                        },
                    }
                });

                //Dialog3
                dialogs.Add(new AdminBRO.Dialog
                {
                    id = 5,
                    replicas = new List<AdminBRO.DialogReplica>
                    {
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Happy,
                            message = "Master, I think we’re ready to expand. This location would be perfect for a new camp!",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Adriel,
                            animation = AdminBRO.DialogCharacterAnimation.Angry,
                            message = "This location is in my domain, dog. I’d rather it remain campless. What do we have here?",
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
                            message = "Ah, you’re awake, Overlord! I guess it’s been too long. Don’t you remember me?",
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
                            message = "How’s this for a reminder?",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right,
                            cutIn = "Cut-in 5: Adriel kissing Overlord’s helmet"
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Angry,
                            message = "What. The fuck. Are you doing?!",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.UlviWolf,
                            animation = AdminBRO.DialogCharacterAnimation.Angry,
                            message = "Listen, you big titty bitch. You’re not fooling anyone with those melons. I can see that you have a dick.",
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
                            message = "I’m an angel, little puppy. I can have a dick or I can get rid of it if I so desire. Or if anyone else desires.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Adriel,
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            message = "This is not why I’m here. Heed my warning, Overlord. I will not surrender to you easily. If you want to defeat me, you’d have to work very, very hard.",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right
                        },
                    }
                });

                //Sex2
                dialogs.Add(new AdminBRO.Dialog
                {
                    id = 6,
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
                            message = "My clit is throbbing under your touch, it feels so good!",
                            cutIn = "Cut-in 6: pussy rubbing"
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            message = "Ahh~ I’m gonna burst!",
                            cutIn = "Cut-in 6: pussy rubbing"
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            message = "Wow~ You made me cum so hard with just your fingers! Do you want to be my Master?",
                            cutIn = "Cut-in 7: Ulvi’s orgasm"
                        },
                    }
                });

                //Sex3
                dialogs.Add(new AdminBRO.Dialog
                {
                    id = 7,
                    replicas = new List<AdminBRO.DialogReplica>
                    {
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            message = "Oh, Master, I’m so wet… Your dick slides in so easily!"
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            message = "Ah~ You pulled my tail out! I’m so embarrassed… But it feels so good!",
                            cutIn = "Cut-in 8: taking the tail out"
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            message = "Cum with me, Master! Ummh~ I want to take your hot load…"
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            animation = AdminBRO.DialogCharacterAnimation.Love,
                            message = "Ahh~ Ahh! It feels so hot… Thank you for treating me, Master!",
                            cutIn = "Cut-in 9: creampie"
                        },
                    }
                });
            }
        }
    }
}
