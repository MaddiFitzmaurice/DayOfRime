// VARIABLES
VAR state = 0

// START HERE
-> choice1

// FUNCTIONS
==function AlterState(x)
    ~ state = state + x

// STORY
==choice1==
Hello there.

Choice 1: What do I choose?
    * good choice
        ~ AlterState(1)
    * bad choice
        ~ AlterState(-1)
    
- -> choice2

==choice2==
Hello again woah.

Choice 2: What do I choose?
   * good
        ~ AlterState(1)
    * bad
        ~ AlterState(-1)
- -> END