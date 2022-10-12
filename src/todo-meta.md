## Next prio
- unify text boxes (use RTF for all)

- allow saving changes to items
	- any item's text after editing becomes RTF (because the editor will be RTF for all), so, PlainText > RTF, HTML > RTF. That formatted text is stored in DB as raw data[]
	- the plain text value is also stored in DB for searches
	- therefore the DataType becomes 'InitialDataType', because it may no longer be relevant

## Unordered ideas
- adding due dates for items
- adding reminders
- add 'Add item' view
- add tagging/coloring of items
- add linking of items with each other (sub-tasks)
- add multilist view
- add hotkey to create new free text item
- add searches in items
- ensure no duplicates added by comparing new item with other items by plain text
- add setting for:
	- smooth scroll
	- opacity level

## Done
- add hotkey to bring list to top
- figure out the format exceptions that happen when writing to RTF (apparently non-issue, https://github.com/xceedsoftware/wpftoolkit/issues/1465)