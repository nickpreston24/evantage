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
    - [x] Each 'full day' is a collection of tasks grouped by similar weight (priority x duration x time passed since creation)
    - [ ] Optional: Might a bell curve of weight distribution help?
    - [x] The algorithm will do its best to slot the tasks so that no day is too large (limit: 5 tasks; 1x P1, 1x P2, 2x P3, 2x P4).  
      - [ ] This can always be configured as I get better at completing tasks.
### Other todos

- [ ] See if there's another alternative for Airtable that's free and local.  I would like to not have to pay if there's a FOSS.
  - [ ] Alternatively, use pocketbase and use the .net pocketbase client.
    - [ ] See if there's an easy golang+pocketbase web app tutorial.  I might be interested in switching, b/c I"m bored and I just wanna make things.
- [ ] Scaffold each of these Ideas as method stubs.


## Features

### Call tracking
- [x] Create a button for each telephone number you upload to google sheets, csv, etc.
  - [ ] In addition to a call btn, I'd like a link for "Add as Contact" on my phone, if possible. (idk how, but I'd imagine highlighting it will work) - p4.
- [ ] Provide a link to google maps for addresses. - p2
  -  I'd love to be able to upload a csv of leads and have the address, call button ready to go.
- [ ] Address lookup: p4
  - [ ] Parse the years been in business - p4
  - [ ] Parse the hours of operation (so you're not wasting calls) - p4
    - [ ] Grey out (disable) the call button, if not within hours. - p4
    - [ ] Using htmx polling, refresh these call buttons, checking to see if we're within hours.
    - [ ] Maybe provide a status: "Available" or something.
### Leads Generation

#### Leads Scraper
  - [ ] Save cssselectors for sites with this kind of info, like the sales tax office, the online phone books, etc.
  - [ ] Scrape Addresses, phones, hours of operation from Google.
    - [ ] May have to use ScrapingBee, so not banned.
  - [ ] Prevent duplicates by comparing phone numbers.

#### Feature: Best time to call
  - [ ] Create array of time slots (30min default) for the day
  - [ ] Create array of time Slots for current hours.
  - [ ] Subtract the two arrays (use IComparable, and if necessary, NodaTime for easy compares)
    - [ ] Overload the` Equals()`
    - [ ] Overload `ToString()` for compares.
    - [ ] Default to UTC, but verify w/ research first.

### OCR
- .net libraries to try:
  - These were a bust...
  - [x] [tesseract](https://dev.to/mhamzap10/how-to-use-tesseract-ocr-in-c-9gc)
  - [x] [ironocr](https://ironsoftware.com/csharp/ocr/examples/simple-csharp-ocr-tesseract/?utm_source=nuget&utm_medium=organic&utm_campaign=readme&utm_content=supportanddocs)
  - `tesseract.js` works.
  - `tesseract` on linux works.
- If OCR is reasonably successful:
  - [ ] Parse out phone numbers, business ids, and addresses where possible.
    - [ ] *P1* Add a button to your existing textarea, which takes the text as-is and saves to Airtable.  Can be used for imports from tesseract.js, local drive, or copy-paste!
  - [ ] cross ref with anything useful you find in the google maps api.
  - [ ] Update your db with new or existing locations.

### Airtable

- [ ] Create a new Leads table to hold onto leads, temporarily.
- [ ] Copy each query string (GET|POST, etc.) into consts.
- [ ] Recycle the airtable service regex to parse out the parts of ea. query string.
- [ ] Goals:
  - [ ] GET/Create new lead
  - [ ] POST/Update existing lead
  - [ ] Bonus: Use the existing CSV reader to upload your local CSV file contents to Airtable.
  - [ ] Create a dedicated route in Razor for Airtable.

#### Web Clippers

- [ ] Sales tax office Web clipper
- [ ] Generic FFL leads clipper
  - Snags url, title, etc.
  - Updates the 'lead sources' table

### Cold Calling

- [ ] Have the call button update Airtable with 'call_count++', set `last_call_date` to `DateTime.Now` and have it disabled for that record when `call_count == 0 || DateTime.Now >= last_call_date`.  Calling 4-6 mo. later is fine, and we don't have to reset the call count, necessarily.  
- [ ] Create a field for when to call back again.  This can be a simple datebox, but I like the idea of having a few presets like `1 Day`, `1 Month`, `6 Months`, etc.

### Google Maps

- [ ] Integrate basic gmap
- [ ] Look for way to show heatmap of already visited places.
- [ ] Mark already visited pins as red, default green.
- [ ] Icon for "find on google maps"
  - [ ] Takes the user to google maps (embedded, like your react map)
  - [ ] "Doors Deluxe" (nice title! ^_^)
  - [ ] Once the user lands on Maps, alpinejs verfies the `current_phone` matches the current Google Map Pin's phone number. 
  - [ ] In Map, highlight the phone and address.
  - [ ] Maybe add a hoverable modal (daisy dropdown) for calling the phone?
    - [ ] And of course, hook that into an `hx-get` to update the call count for that number! ^_^

### Statuses, Types and Descriptions
- [ ] Create a special base `Enumeration` class called `Alias` & `IAlias` interface.
  - [ ] takes `string aliases` as a cotr. param
  - [ ] The goal is to:
      1.   Map these aliases to airtable Selects & MultiSelects (or other entities)
      2.   If an alias is discovered in a description (.contains()), or is a close match, suggest and edit in the table.  
           1.   Note: use `x-init` when loading and calling the `CheckForAliases()` handler.
    - [ ] Create `AirtableAlias` subclass
      - [ ] Create extensions like `ToAlias(string aliases[]})` that support conversion to defined `IAlias`es.
      - [ ] fields
        - [ ] base_id
        - [ ] table_id
        - [ ] record_id (the one that comes from Airtable, if it's possible to get it from a Select)


### Stats Page
* Want to know:
1.  How many calls for the Day
2.  Calls for the weekk
3.  How much to make up for the week.
4.  Ideally, show percent completion for: (Doors, Cold Calls, Leads Generation)

### Current Call Page
- [ ] Create a `CurrentCall` Page.
- [ ] Redirect after clicking the call button.
- [ ] Add a call button to it.
- [ ] Create an edit form so you can update the Lead while in a call.
- [ ] Notes should be prominently displayed.
- [ ] Statuses:
  - [ ] Voicemail
  - [ ] Textback
  - [ ] Callback  
    - [ ] Callback Time (grab dropdown/calendar from your OrchardCore)
      - [ ] Set status of Lead (in Airtable) as 'Callback'
      - [ ] If current state is already 'Callback' in Airtable, increment the call_count, and set state to 'CalledBack' (or equivalent)
      - [ ] test on your mobile, using your own Webex account (or anything that'll call your mobile)

### Google Search Menus

- [ ] Add 'Search Google' option when hovering over an:
  - [ ] Address & Zip
  - [ ] Website
- [ ] Add 'Search Phone' option when hovering over phones
  - [ ] Use an online phone book, if possible.
  - [ ] fallback, search Google for the phone # and see what business pops up.
- [ ] Bonus:
  - [ ] What if I could, with each Google Maps search results, add *similar businesses*?
    - I got the idea from "Abundant Seed Investments".
    - Google already recommends them.  Maybe Waze also has an API I can use.
- Consider writing a MySQL service in Railway, or perhaps even Turso.
  - [ ] Investigate: C# Turso driver.

### Services

- [ ] Add AirtableServices for:
  - [ ] Leads (Interactions)
  - [ ] Opportunities (Appointments, Followups, Callbacks, textbacks, closes, etc.)

### Webex
- [ ] setup on linux
- [ ] try calling yourself

### Todoist

- [ ] Create task from Lead - p1
  - [ ] Create new todoistTask using Todoist API, under project 'EMG'
  - [ ] Have todoist set a reminder to blow up my phone with. :) p1
  - [ ] Set label to whatever the current (relevant) status is (e.g.`@callback`, `@appointment`, etc.) - p3


### Bash
- [ ] OPTIONAL: add an input box for running a bash cmd on railway.

### SQLITE

- [ ] maintain a locel repo & database for syncing new leads coming from OCR, regex, etc.


### Priority
- [ ] Create records in a sqlite db so you can stop using the excel.
- [ ] query and update all records for the current day to Airtable using their PUT.
  - [ ] Note: probably no way to use bulk upload, so need to wait ~2.5 seconds per upload.
  - [ ] May need to write this as a CLI option in your existing app.
  - [ ] --upload-air Leads.db...
- [ ] Finally, Get all Leads and render on Home screen.


### Auto form entry
- [ ] Find out how to copy/paste into a form from the browser/clipboard
- [ ] Add a clipboard btn that can copy ClientVine's Contact shape.

### Call Details - p2

- [ ] Under the Call button, I'd like the following
  - [ ] Call count (# of times I called this #) - p1
  - [ ] Date of last call in red. - p1
  - [ ] Status of last call (callback, email, text, hung up, DNC, disconnected number, etc) - p2
  - [ ] A link to the full call history `/Call/History/12345`. - p3
  - [ ] Time till next callback (for Voicemails and callback Statuses) - p4
  - [ ] A link to clientvine that auto-copies all relevant details. - p2
  - [ ] p2 - I'd like to see how many times I've called back in the call history.
  
### Lead Details
- [ ] Under business name, I'd like the following
  - [ ] Website link
  - [ ] Notes (details expander)
  - [ ] The phone # related to this Lead.
  - [ ] Contacts list (details exapnder or menu)


### Leads Search

- [ ] Save recent searches to localStorage after hitting Submit, right before `hx-get` fires.  clg it to test it; show it in a dropdown, take 5, if possible ...  - p4.

### Special Features todo page

- [x] Read `Readme.md` as a `string[]` - p2
- [ ] Extract out `p1`s thru `p4` and sort by priority, desc on the page. - p2
- [x] readonly, for now.  Easy way to track. :)
- [ ] unlabeled tasks at the bottom labeled (in red) "unsorted"
- [ ] tasks that are checked, show at the bottom, disabled or crossed off. p3

### Offline Mode

- [ ] run yarn & Test your asp-fallback scripts (htmx, alpinejs, tw, etc.) (be sure to save your wifi password, first)

### To Fix

1. Having a deploy issue right after adding /Logs: [last good commit](https://github.com/nickpreston24/evantage/commit/f49765e39690fec8d6257a84698fb5ead402187c?diff=unified&w=1#diff-16356dd7d493d3f455c053c628f0ea4266db867dc32218d24dba0422b1f16182)

### side by side / Bugs Fixed page

1. Create a /bugs/fixed collage page.  Simple grid or bento of your best bug fixes.
2. Create a /porfolio path?
3. Make a side-by-side with your code vs their code, showing your fix.  This can be screengrabs like your Todoist fixes, mockups of stuff from old jobs, etc.  stash sidebyside screenies under /img.

### Portfolio page
1. Create a scrolling, virtual resume that takes users through a timeline, describing what you do and where you're at now.


### Job Hunt 

- [ ] Create a simple Service and 2 forms and 1 grid to track your applications
- [ ] Create a basic stats panel and maybe a graph showing trends.
- [ ] Create a grid specifically for applications over 30 days old (for reapplying)
- [ ] Copy existing, raw airtable data and use a regex to parse it and load it to your Mysql server.

### Project-Specific Grids

- [ ] Create a re-useable grid that accepts a project name in the curl, and title of grid, and loads only that Todoist project.
- [ ] Start using this for Job Hunting.


### Show recently added Todoist and Older tasks and ask if they are done.

I want to be able to see my latest Todos and tick them off, without creating a Reschedule problem.

- [ ] Add a 'recently added' view
- [ ] Add another 'older tasks' view, sorted by Priority
- [ ] Fix the searchbar for the main table view.
- [ ] Add [FuzzySearch](https://github.com/JakeBayer/FuzzySharp) powered bar.


### May 6th, Goals

- [ ] Contacts form and table.
- [ ] Exclude Todoist tasks & projects labeled with "fun", "someday", or the like.  Include some things, like certain gun tasks and most of my buy list.
- [ ] Create a bumping method in TodoistService that bumps priority up to p3 if it's p4 and 30 days old, p2 if 60, and p1 if 90.  This will help you sort things out in the long run.
- [ ] Create a sample stats (progress %) list showing the ratios of your job applications.
  - [ ] *Bonus* Animate as folding tiles (like venetian blinds, or Portal tiles)
- [ ] *Bonus* I want to have all my emails back to 0 in Gmail, and have a basic Puppeteer scraper API ready to save prospective Junior level jobs for application.  These can be parsed from the emails themselves (links to the jobs) and can be 
- [ ] Word wrap the Job Hunt descriptions (names).
- [ ] Update Name to Description in Job Hunt? (they appear to be flipped)
- [ ] Prioritize P1 tasks in the Full Day / Schedule.
- [ ] Create an Instacart Page displaying simple runs calculator and sample (real) calculations.
- [ ] Create simple markdown file search; any links to path opens that md file.
- [ ] Create a method in ReadmeService that lets you associate Readme.md tasks with pages, and list them in a small table panel.
- [ ] Create a basic login service using mysql and email/pwd.  It should also allow a 'demo' mode for interviewers.  This demo mode can have roles/access, too.
- [ ] Create a linux daeomon in C# that does the following:
  - [ ] Sends you and Mom an email, with a list of todoist or readme tasks completed today.
  - [ ] Sends a notification at breakfast, lunch, dinner.
  - [ ] Sends me a random scripture or TPOT from all the .json and .md files I have on drive.
  - [ ] Performs the Priority bumping
- [ ] Use the Bash command + grepper to find all Git repos, and read their statuses in a GitHub Service.
- [ ] Create an ObsidianService.cs that uses IMarkdownService and can do a simple live search on files using Grepper.  Make a panel that can easily render markdown.
- [ ] Create a typeahead on main Index.cshtml page that OmniSearches through all cshtml files and their code content for what you typed, and provides links, so you can jump to the relevant page.  See OpticsPlanet for inspiration.
  - [ ] This requires that you update your RazorRoutesService to properly generate links (with optional tooltips, see associated Model)
- [ ] upgrade your ReadmeService to print out and update specific tasks, on their exact line and SaveAs to README.md.

## Fun Ideas

- [ ] *Optional* Upgrade your Personal Daemon service to watch for code changes in a target dir, and control the Hot Reload process (including rude edits)
- [ ] Colored console logs: https://github.com/tomakita/Colorful.Console - p4
- [ ] Using an existing RegexService, parse all your .CS files for regex101 link s an the regex patterns that follow.  Store to MySql. :)
- [ ] Prioritize Todoist Tasks based on Distributions, like duration, how many times completed (streaks), etc.
  - [ ] https://www.youtube.com/watch?v=V3Qxj2C7rP0
  - [ ] https://www.youtube.com/watch?v=bPFNxD3Yg6U
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


### GMail Support

- [ ] Investigate: https://developers.google.com/api-client-library/dotnet/apis/gmail/v1 - p3
- [ ] https://www.youtube.com/watch?v=0fTSKtPBLUc
- [ ] ..
- [ ] ..
- [ ] ..

### Applications - Generators

- [ ] Form for generating a specific Email greeting for different job titles (Vue)
- [ ] Copy/paste button for emai

### May 9th goals
- [ ] Rescheduler fix: update all your breakfast tasks as done, between the due date and now.  Scedule a new breakfast for tomorrow. Run w/ a button
- [ ] try making a GET request to Airtable using your new chrome extension.
- [ ] tes t your Controllers to see if you can ping them in prod; save down to mysql
- [ ] Finish your first ScriptClassAsTable implementation and add to CodeMechanic so you can use it.
- [ ] Upload all README.md tasks to its own db in Mysql, then render the table from there, not from README.md.  re-Populate on startup, and only show the latest.  This will help prioritize coding goals in one easy place.  Also, make a search box, using your ToString() trick.
- [ ] GET all Jobs from Job Hunt Airtable.
- [ ] Show a random Proverb on /Scriptures.
- [ ] Sort README.md tasks by length of tabs, and by priority.  If no priority exists, default to the count of tabs % 4.
- [ ] Start on a View for your CodeMechanic base and tree structure, versions of nuget pkgs, etc.
- [ ] Start and /Admin folder.