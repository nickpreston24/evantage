# SalesSpy (Formerly, Evantage)

- [ ] Replace the existing Rescheduling feature, if possible
  - _It's really bad. I don't want to reschedule all tasks at once and doing it one at a time is tedious. I'd really like to be able to sort by how long they've been active tasks and how long it will take to do them._

Problem: If you forget to tick off a recurring task, you're out of luck. For some reason, there's no option to check a week's worth of laundry that you forgot to check off.  Instead, only the late one is checked and, if it's recurring, reset to the next occurence.
- [ ] What I would like is for that week's worth of laundry to tick all 7 radio's on the backend, not just one.
  - [ ] Idea: Update the recurring task to non-recurring.
  - [ ] Create 6 duplicates of the now non-recurring task.
  - [ ] Update all of them as done, except the very last one.
  - [ ] Update the last one as recurring, with the original next-date applied.
  
Problem:  The Eisenhower Matrix is great and all, and so are priorities and views, but that's just not enough!  What if we could have our schedule chunked up for us?  I have 250+ tasks that need spread out across X weeks, so how can we do that?

- [ ] Update Todoist service such that:
  - [ ] I can set the length of a task in labels and descriptions
    - [ ] Tags and descriptions may contain units and values
    - [ ] Durations (label and amount) are parsed by regex in a mock 'natural language'
      - [ ] Can parse any natural language from any input box, by using typeaheads.
  - [ ] I can click a button and a randomized week (or more) will be generated
    - [ ] Each 'full day' is a collection of tasks grouped by similar weight (priority x duration x time passed since creation)
    - [ ] Optional: Might a bell curve of weight distribution help?
    - [ ] The algorithm will do its best to slot the tasks so that no day is too large (limit: 5 tasks; 1x P1, 1x P2, 2x P3, 2x P4).  
      - [ ] This can always be configured as I get better at completing tasks.
### Other todos

- [ ] See if there's another alternative for Airtable that's free and local.  I would like to not have to pay if there's a FOSS.
  - [ ] Alternatively, use pocketbase and use the .net pocketbase client.
    - [ ] See if there's an easy golang+pocketbase web app tutorial.  I might be interested in switching, b/c I"m bored and I just wanna make things.
- [ ] Scaffold each of these Ideas as method stubs.

### Fun Ideas

- [ ] on each tab in a cshtml form, call a `hx-get` to save the partially filled form to the db, so that there's always a session to come back to on internet failure.
- [ ] Hyper Grep serivce that can search keywords across multiple drives (incl google drive) asynchronously and give a breakdown of a) files, b) extensions, directories and c) file contents matching each kind. (use multiple greps and set a range of extensions and  key root dirs to search, like Google Drive)
- [ ] Build your own /obsidian tab that uses the hyper grep and renders Markdown in a similar fashion.
- [ ] I want to be able to search for any phrase across my drive and organize my folders better, including backing up to USB.  Linux is great, but greps and shell scripts are syncronous, while C# and golang are async and can utilize mutliple cores.
- [ ] I also want a hypergrep service as a NugetPackage, so I can easily parse for Scriptures inside files (TPOT Links)
- [ ] Dockerize this app - Once I can properly Dockerize my many apps, I should have no trouble keeping them all federated (separated) from each other, while still running on localhost.  That way, I can make all my data AND apps portable.  So far, they are not, and that sucks.  I'm also sick of APIs and paying for Airtable, so I must get crafty and hacky.
- [ ] I also want the ability to dump all Pocketbase data to a USB anytime (like in an emergency)
- [ ] I'd also like an Airtable-like layer on top of Pocketbase, if it exists.  Maintaing data thru Airtable is grand, but expensive.
- [x] I'd also like Todoist integration (WiP)
  > Neo4j is optional at this point.  It was really more effective as a recommendation engine.

- [ ] Use Google Maps to find main streets and businesses off the beaten path (idk, say, 150+ feet away from the road, maybe the corner businesses not facing the main st., etc.)
- [ ] Using the HyperGrep, I'd like to...
  - [ ] Find all occurences of a string.
  - [ ] Edit the first file with that occurrence.
  - [ ] Preview other files it might affect, while typing.
  - [ ] Replacment support for that regex.
  - [ ] Chunked transforms: Find specific lines using a start and ending regex (already built.) and perform specific transforms from start to end.  Usually replacements, but can be something like code generation, mass corrections to malformed data, finding dups (like Airtable does).
  - [ ] Allow user to queue the replacements and transforms.
  - [ ] Update the Hydrolizer to create C# props from alpinejs variables it discovers within script tags.

#### Call tracking
1. Create a button for each telephone number you upload to google sheets, csv, etc.
2. 