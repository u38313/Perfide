// Startpunkt
-> Part_1_Start

// --- ABSCHNITT 1: Der Start ---
=== Part_1_Start ===
# audio: bang_loud
# camera: Grigsby
# speaker: Grigsby
Hey Chief!
You hear that knocking just now? What was that? I really hope Genbu isn't about to break completely apart.

// Hier gibt es nur EINE Wahl. Sie führt per Pfeil (->) sicher zu Teil 2.
+ [Grigsby? Did you not leave to fix the Crane?]
    -> Part_2_Explanation

// --- ABSCHNITT 2: Die Erklärung ---
=== Part_2_Explanation ===
# speaker: You
Grigsby?
Did you not leave some time ago to fix the Crane?

# speaker: Grigsby
Yeah I did, and I came back because I broke a Bolt, and had to get a new one, have you forgotten already?

# event: RevealOutside
# audio: knock_frantic
# audio: speaker_static
# camera: Window

# speaker: Grigsby (Outside)
NO FAED WAAAAAIT
THAT-THATS NOT ME IN THERE, I AM STILL HERE OUTSIDE!

# camera: Grigsby
# speaker: Grigsby
Oh Astra Patronus! Why is there another Astronaut out there AND WHY ARE THEY CLAIMING TO BE ME??
There is nothing around us where they could have come from!
Faed, I'm scared.
Can we please just leave right now and go Home in an instant?

// Wenn dieser Text vorbei ist, kommt die nächste Wahl
+ [*Reach for the Lever*]
    -> Part_3_Confrontation

// --- ABSCHNITT 3: Die Konfrontation ---
=== Part_3_Confrontation ===
# speaker: You
*Reach for the Lever*

# camera: Window
# speaker: Grigsby (Outside)
ARE YOU MAD??? PLEASE DON'T ABANDON ME FAED!
I AM REALLY ME, GRIGSBY
That thing right there with you has to be some kind of Intruder or Monster!

# camera: Grigsby

// WICHTIG: Dieser Bindestrich (Gather) repariert den "Loose End" Fehler!
// Er verbindet den Text oben sicher mit der Auswahl unten.
-

+ [Silence, both of you...]
    -> Part_4_Conclusion
// --- ABSCHNITT 4: Das Ende ---
=== Part_4_Conclusion ===
# speaker: You
Silence, both of you,... I need to think.
Ugh, I can't wrap my head around this right now.
I'm gonna have a Chat with each of you, and determine what to do afterwards.
And NO funny Business from either of you.

// Und hier endet das Gespräch komplett
-> END