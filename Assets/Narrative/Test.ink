// CONSTANTS
CONST UPPER_LIM = 2
CONST LOWER_LIM = -2

// VARIABLES
VAR state = 0

// START HERE
-> choice1

// FUNCTIONS
==function CheckAlterState(x)==
    { 
        // if state between very good and very bad
        - state > LOWER_LIM && state < UPPER_LIM:
            ~ AlterState(x)
        
        // else if state at very bad but good choice was made
        - state == LOWER_LIM && x > 0:
            ~ AlterState(x)
        
        // else if state at very good but bad choice was made
        - state == UPPER_LIM && x < 0:
            ~ AlterState(x)
    }
==function AlterState(x)==
    ~state = state + x
    
// STORY
==choice1==
Hello there.

Choice 1: What do I choose?
    * good 1
        ~ CheckAlterState(1)
    * bad 1
        ~ CheckAlterState(-1)
- -> choice2

==choice2==
Hello again woah.

Choice 2: What do I choose?
    * good 2
        ~ CheckAlterState(1)
    * bad 2
        ~ CheckAlterState(-1)
- -> choice3

==choice3==
Hi...

Choice 3: What do I choose?
    * good 3
        ~ CheckAlterState(1)
    * bad 3
        ~ CheckAlterState(-1)
- -> choice4

==choice4==
Please let me leave.

Choice 4: What do I choose?
    * good 4
        ~ CheckAlterState(1)
    * bad 4
        ~ CheckAlterState(-1)
- -> choice5

==choice5==
Ima bout to spoil Bloodborne if you keep going...
Choice 5: What do I choose?
    * good 5
        ~ CheckAlterState(1)
    * bad 5
        ~ CheckAlterState(-1)
- -> END