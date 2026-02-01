// Globale Variablen
VAR know_crane_fixed_log = false 
VAR asked_g1_crane = false
VAR asked_g2_crane = false

// Einstiegspunkte
=== Interrogate_G1 ===
-> G1_Hub

=== Interrogate_G2 ===
-> G2_Hub

// --- GRIGSBY INSIDE (G1) ---
=== G1_Hub ===
# camera: Grigsby
# speaker: Grigsby (Inside)
{not G1_intro: Huh? What is it Chief? | What else?}
-> G1_Options

= G1_intro
I'm listening.
-> G1_Options

= G1_Options
// Option 1
+ [Remember what Cycle you joined?]
    # speaker: Faed
    Remember what Cycle you joined me on the Genbu?
    # speaker: Grigsby (Inside)
    Uhh, gosh... let me think.
    Oh right, it was the 93rd cycle last Year! I remember it like yesterday!
    # speaker: Faed
    Yeah... it was chaotic.
    -> G1_Hub

// Option 2
+ [What did you do on Earth?]
    # speaker: Faed
    Tell me, what did you do, while you were still on the old Earth?
    # speaker: Grigsby (Inside)
    Sure! I was working in an old Antiquity store!
    Gosh, thinking back, all I would do is spend my free Time reading the Books of old...
    # speaker: Faed
    Grigsby. Sorry to Interrupt you, but that is plenty.
    # speaker: Grigsby (Inside)
    Oh right, my baaad.
    -> G1_Hub

// Option 3
+ [Why are you still inside?]
    # event: HeardInsideStory
    # speaker: Faed
    Why are you still inside?
    # speaker: Grigsby (Inside)
    Huh? I already told you that I broke one of the necessary Bolts for the Crane...
    I'm sorry I failed you Chief... But check the Logs, it's still broken.
    -> G1_Hub

// Option 4
+ [What do you think is going on?]
    # speaker: Faed
    What do you think is Going on?
    # speaker: Grigsby (Inside)
    I-... I have no Idea... I don't know why there is another... me out there.
    But I have a really bad feeling about this...
    -> G1_Hub

// Option 5 (LOCKED - Nur wenn Log gelesen wurde)
+ {know_crane_fixed_log} [The crane is fixed...]
    ~ asked_g1_crane = true
    # speaker: Faed
    The logs say the crane is fixed...
    # speaker: Grigsby (Inside)
    Huh? Wait what? But I'm in here and have never fixed it.. that can't be...
    No wayy.. did that... thing out there really fix it?
    
    // Check for Ending Trigger
    { asked_g2_crane: -> Trigger_Ending | -> G1_Hub }

+ [Leave Conversation]
    -> END


// --- GRIGSBY OUTSIDE (G2) ---
=== G2_Hub ===
# camera: Window
# speaker: Grigsby (Outside)
{not G2_intro: Faed! Please let me in! | Just open the door!}
-> G2_Options

= G2_intro
...
-> G2_Options

= G2_Options
+ [Remember what Cycle you joined?]
    # speaker: Faed
    Remember which Cycle you joined me on the Genbu?
    # speaker: Grigsby (Outside)
    Hmm, uhh...
    Damn it all! I can't remember it right now.. But it's just because of the Stress!
    -> G2_Hub

+ [What did you do on Earth?]
    # speaker: Faed
    Tell me, what did you do on Earth?
    # speaker: Grigsby (Outside)
    Easy! I worked at an old Antiquity store for about 4 Years, until it sadly went bankrupt...
    -> G2_Hub

+ [Why are you still Outside?]
    # event: HeardOutsideStory
    # speaker: Faed
    Why are you still Outside?
    # speaker: Grigsby (Outside)
    Huh why? I just finished repairing the Crane ofcourse!
    Just check the logs, you'll see I repaired it!
    -> G2_Hub

+ [What is happening?]
    # speaker: Faed
    What do you think is Happening?
    # speaker: Grigsby (Outside)
    I think you let some sort of deceiving invader inside!
    You really need to get rid of this intruder before it gets rid of you!
    -> G2_Hub

// Option 5 (LOCKED)
+ {know_crane_fixed_log} [The crane is fixed...]
    ~ asked_g2_crane = true
    # speaker: Faed
    The logs say the crane is fixed.
    # speaker: Grigsby (Outside)
    That's what I told you! Thanks to your many lessons, I've improved!
    
    // Check for Ending Trigger
    { asked_g1_crane: -> Trigger_Ending | -> G2_Hub }

+ [Leave Conversation]
    -> END

// --- DAS ENDE ---
=== Trigger_Ending ===
# event: EnableGunAndLever
# speaker: Grigsby (Outside)
FAEED, I don't want to rush you, but my OXYGEN IS RUNNING OUT SOON!
# speaker: Grigsby (Inside)
NO.. PLEASE DON'T! If you let it in, It will definetly hurt the both us.
# speaker: Faed
No... In all my Years... I have never seen anything like this...
I truly do not know which one you are...
If I screw things up... I hope you will be able to forgive me...

// Hier wird das Ink beendet, aber die Pistole und der Hebel sind jetzt aktiv!
-> END