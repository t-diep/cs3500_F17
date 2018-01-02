Design
-When first starting this assignment, we used the PS6Skeleton as a starting point
to set up most of the GUI since some of the base requirements were already done
for us (i.e. Handle Multiple Spreadsheets and Closing the Spreadsheet).  Then, 
we took a lot of time to think about how to connect the text box contents and 
set them to the cells in the panel.  We definitely created several fields 
to make the process doable such as a Spreadsheet from PS5 and a Formula from PS4.
We tried to keep in mind on good software practice principles as we were doing
this project.    

During fall break, we started and implemented the base requirements of the
assignment.  Once fall break was over, we started the extra features.  

One thing we want to mention is how we tried to implement the multiple 
cell selection extra feature.  We realized how long we took (more than 2 hours!)
to implement this extra features but the deadline was getting close, so it 
was wise to omit this idea.  Nonetheless, we have learned so much along the way!


Anyway, enjoy our spreadsheet program and the extra features we put in it! Thank you!

~Tony and Sona

Extra Features

UNDO/REDO CHANGES
-We added undo/redo options to where if you want to revert to the previous change
or the change after the current one.  On the "Edit" menu item on the top, there 
contains the "Redo" and "Undo" items.  These options will at first be disabled if
there are no existing modifications to the spreadsheet already, so make sure you
have made at least one modification in order to use these functionalities.

WATERMARK
-We spent a lot of time figuring out how to implement this to find the "shortest"
solution, and after a couple of minutes later (thanks to Google and StackOverflow), 
we found it.  So thank you StackOverflow for the solution.  When clicking on an 
empty cell on the panel, you will notice the text box on the top right corner will 
have a placeholder text indicating where to enter in contents.  When entering contents
in the text box, the placeholder text will dissappear.  Pretty neat.

CHANGE BACKGROUND COLORS FOR SPREADSHEET
-Under the View Menu, you can change the background color of the spreadsheet.  Choose
from three other kinds (aside from default background):  Excel, Tony Edition, and Sona Edition.
We'll leave it to you, grader, to find out what our editions look like... In addition, 
when clicking on a theme to change, there will be a checkmark to the left of it, indicating
that the current theme that you just clicked is the one selected.  Click on it again, 
and the default background will show as well as the checkbox shown on the "Default" option.
If the "Default" selection is deselected, nothing changes (the checkbox will remain there).

ARROW KEYS AND SPACEBAR FUNCTIONALITY
-The focus is set by default to the spreadsheet panel.  If you do not feel like 
using the mouse to move the cursor to the text box to change contents, you can 
press the Spacebar key to change the focus to the text box and enter in contents 
afterwards.  
-You can also use the arrow keys (up, down, left, right) to navigate from one cell 
to another cell without moving your mouse.  We have covered cases in which 
the user can try and navigate a cell outside of the range A1-Z99.  


