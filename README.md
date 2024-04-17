# Evantage Sales app (TBD - Change title)

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
        - [ ] T&D are parsed by regex in a mock 'natural language'
        - [ ] Can do typeaheads in any input box as well.
      - [ ] I can click a button and a randomized week (or more) will be generated
        - [ ] Each 'full day' is a collection of tasks grouped by similar weight (priority x duration x time passed since creation)
        - [ ] Optional: Might a bell curve of weight distribution help?
        - [ ] The algorithm will do its best to slot the tasks so that no day is too large (limit: 5 tasks; 1x P1, 1x P2, 2x P3, 2x P4).  
          - [ ] This can always be configured as I get better at completing tasks.