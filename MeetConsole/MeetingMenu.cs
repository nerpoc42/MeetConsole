namespace MeetConsole;

public class CategorySelectionMenu : Menu
{
    public MeetingCategory Category { private set; get; }

    public CategorySelectionMenu()
    {
        Options = new List<MenuOption>
        {
            new(() => "CodeMonkey")
            {
                Action = () =>
                {
                    Category = MeetingCategory.CodeMonkey;
                    return true;
                }
            },
            new(() => "Hub")
            {
                Action = () =>
                {
                    Category = MeetingCategory.Hub;
                    return true;
                }
            },
            new(() => "Short")
            {
                Action = () =>
                {
                    Category = MeetingCategory.Short;
                    return true;
                }
            },
            new(() => "TeamBuilding")
            {
                Action = () =>
                {
                    Category = MeetingCategory.TeamBuilding;
                    return true;
                }
            }
        };
    }
}

public class TypeSelectionMenu : Menu
{
    public MeetingType Type { private set; get; }

    public TypeSelectionMenu()
    {
        Options = new List<MenuOption>
        {
            new(() => "In Person")
            {
                Action = () =>
                {
                    Type = MeetingType.InPerson;
                    return true;
                }
            },
            new(() => "Live")
            {
                Action = () =>
                {
                    Type = MeetingType.Live;
                    return true;
                }
            }
        };
    }
}

public class MeetingAttendeesMenu : Menu
{
    private readonly int _meetingId;

    private bool AddScheduledPerson()
    {
        var personName = MenuUtils.ReadString("name");
        if (personName == null)
        {
            Console.WriteLine("Unable to add person to the meeting");
            return false;
        }

        var date = MenuUtils.ReadDate("attendance date");
        if (date == null)
        {
            Console.WriteLine("Unable to add person to the meeting");
            return false;
        }

        if (AttendeeCollection.AddToMeeting(personName, _meetingId, (DateTime)date)) return true;
        Console.WriteLine("Unable to add person to the meeting");
        return false;
    }

    private bool RemovePerson()
    {
        var personName = MenuUtils.ReadString("name");
        if (personName == null)
        {
            return false;
        }

        if (AttendeeCollection.RemoveFromMeeting(personName, _meetingId)) return true;
        Console.WriteLine("Unable to remove person from the meeting");
        return false;
    }

    public MeetingAttendeesMenu(int meetingId)
    {
        _meetingId = meetingId;

        Headline = () => string.Join('\n', MeetingCollection.GetMeeting(_meetingId).Attendees);

        Options = new List<MenuOption>
        {
            new(() => "Finish Editing"),
            new(() => "Add New Person")
            {
                Action = AddScheduledPerson,
                MenuCloses = false
            },
            new(() => "Delete Person")
            {
                Action = RemovePerson,
                MenuCloses = false
            }
        };
    }
}

public class MeetingEditingMenu : Menu
{
    public MeetingEditingMenu(int meetingId)
    {
        Headline = MeetingCollection.GetMeeting(meetingId).ToString;

        Options = new List<MenuOption>
        {
            new(() => "Finish Editing"),
            new(() => "Change Name")
            {
                Action = () =>
                {
                    var name = MenuUtils.ReadString("name");
                    if (name == null)
                    {
                        return false;
                    }

                    MeetingCollection.GetMeeting(meetingId).Name = name;
                    return true;
                },
                MenuCloses = false
            },
            new(() => "Change Description")
            {
                Action = () =>
                {
                    var description = MenuUtils.ReadString("description");
                    if (description == null)
                    {
                        return false;
                    }

                    MeetingCollection.GetMeeting(meetingId).Description = description;
                    return true;
                },
                MenuCloses = false
            },
            new(() => "Change Responsible Person")
            {
                Action = () =>
                {
                    MeetingCollection.GetMeeting(meetingId).ResponsiblePerson = MenuUtils.ReadString("name");
                    return true;
                },
                MenuCloses = false
            },
            new(() => "Change Category")
            {
                Action = () =>
                {
                    var menu = new CategorySelectionMenu();
                    menu.Serve();
                    MeetingCollection.GetMeeting(meetingId).Category = menu.Category;
                    return true;
                },
                MenuCloses = false
            },
            new(() => "Change Type")
            {
                Action = () =>
                {
                    var menu = new TypeSelectionMenu();
                    menu.Serve();
                    MeetingCollection.GetMeeting(meetingId).Type = menu.Type;
                    return true;
                },
                MenuCloses = false
            },
            new(() => "Change Start Date")
            {
                Action =
                    () =>
                    {
                        MeetingCollection.GetMeeting(meetingId).StartDate = MenuUtils.ReadDate("start date");
                        return true;
                    },
                MenuCloses = false
            },
            new(() => "Change End Date")
            {
                Action = () =>
                {
                    MeetingCollection.GetMeeting(meetingId).EndDate = MenuUtils.ReadDate("end date");
                    return true;
                },
                MenuCloses = false
            },
            new(() => "Change Attendees")
            {
                Action = () =>
                {
                    new MeetingAttendeesMenu(meetingId).Serve();
                    return true;
                },
                MenuCloses = false
            },
            new(() => "Delete Meeting")
            {
                Action = () =>
                {
                    var personName = MenuUtils.ReadString("your name");
                    if (personName == null)
                    {
                        return false;
                    }

                    if (MeetingCollection.RemoveMeeting(meetingId, personName)) return true;
                    Console.WriteLine("Unable to remove the meeting");
                    return false;
                }
            }
        };
    }
}

public class MeetingSelectionMenu : Menu
{
    private List<int> _meetingIds;

    private bool ListAllMeetings()
    {
        Console.WriteLine("---");

        for (var i = 0; i < _meetingIds.Count;)
        {
            if (!MeetingCollection.Meetings.ContainsKey(_meetingIds[i]))
            {
                _meetingIds.RemoveAt(i);
                continue;
            }

            var meeting = MeetingCollection.GetMeeting(_meetingIds[i]);
            Console.WriteLine($"ID: {_meetingIds[i]}");
            Console.WriteLine(meeting);
            Console.WriteLine("---");
            ++i;
        }

        return true;
    }

    public MeetingSelectionMenu()
    {
        _meetingIds = MeetingCollection.Meetings.Keys.ToList();

        Options = new List<MenuOption>
        {
            new(() => "Return"),
            new(() => "List Meetings")
            {
                Action = ListAllMeetings,
                MenuCloses = false
            },
            new(() => "Remove Filter")
            {
                Action = () =>
                {
                    _meetingIds = MeetingCollection.Meetings.Keys.ToList();
                    return true;
                },
                MenuCloses = false
            },
            new(() => "Filter by Name")
            {
                Action = () =>
                {
                    var filterName = MenuUtils.ReadString("name");
                    if (filterName == null)
                    {
                        return false;
                    }

                    _meetingIds = _meetingIds.FindAll(m =>
                    {
                        var name = MeetingCollection.GetMeeting(m).Name;
                        return name != null && name.ToLower().Contains(filterName.ToLower());
                    });

                    return true;
                },
                MenuCloses = false
            },
            new(() => "Filter by Description")
            {
                Action = () =>
                {
                    var filterDescription = MenuUtils.ReadString("description");
                    if (filterDescription == null)
                    {
                        return false;
                    }

                    _meetingIds = _meetingIds.FindAll(m =>
                    {
                        var description = MeetingCollection.GetMeeting(m).Description;
                        return description != null && description.ToLower().Contains(filterDescription.ToLower());
                    });

                    return true;
                },
                MenuCloses = false
            },
            new(() => "Filter by Responsible Person")
            {
                Action = () =>
                {
                    var filterName = MenuUtils.ReadString("name");
                    if (filterName == null)
                    {
                        return false;
                    }

                    _meetingIds = _meetingIds.FindAll(m =>
                    {
                        var name = MeetingCollection.GetMeeting(m).ResponsiblePerson;
                        return name != null && name.ToLower().Contains(filterName.ToLower());
                    });

                    return true;
                },
                MenuCloses = false
            },
            new(() => "Filter by Category")
            {
                Action = () =>
                {
                    var menu = new CategorySelectionMenu();
                    menu.Serve();
                    var filterCategory = menu.Category;

                    _meetingIds = _meetingIds.FindAll(m => MeetingCollection.GetMeeting(m).Category == filterCategory);
                    return true;
                },
                MenuCloses = false
            },
            new(() => "Filter by Type")
            {
                Action = () =>
                {
                    var menu = new TypeSelectionMenu();
                    menu.Serve();
                    var filterType = menu.Type;

                    _meetingIds = _meetingIds.FindAll(m => MeetingCollection.GetMeeting(m).Type == filterType);
                    return true;
                },
                MenuCloses = false
            },
            new(() => "Filter by Start Date")
            {
                Action = () =>
                {
                    var menu = new DateFilterSelectionMenu();
                    menu.Serve();
                    var predicate = menu.Predicate;

                    _meetingIds = _meetingIds.FindAll(m =>
                    {
                        var date = MeetingCollection.GetMeeting(m).StartDate;
                        return date != null && predicate((DateTime)date);
                    });
                    return true;
                },
                MenuCloses = false
            },
            new(() => "Filter by End Date")
            {
                Action = () =>
                {
                    var menu = new DateFilterSelectionMenu();
                    menu.Serve();
                    var predicate = menu.Predicate;

                    _meetingIds = _meetingIds.FindAll(m =>
                    {
                        var date = MeetingCollection.GetMeeting(m).EndDate;
                        return date != null && predicate((DateTime)date);
                    });
                    return true;
                },
                MenuCloses = false
            },
            new(() => "Filter by Number of Attendees")
            {
                Action = () =>
                {
                    var menu = new IntFilterSelectionMenu();
                    menu.Serve();
                    var predicate = menu.Predicate;

                    _meetingIds = _meetingIds.FindAll(m =>
                    {
                        var count = MeetingCollection.GetMeeting(m).Attendees.Count;
                        return predicate(count);
                    });
                    return true;
                },
                MenuCloses = false
            },
            new(() => "Edit Meeting")
            {
                Action = () =>
                {
                    var meetingId = MenuUtils.ReadInt("meeting ID");
                    if (meetingId == null)
                    {
                        return false;
                    }

                    new MeetingEditingMenu((int)meetingId).Serve();
                    return true;
                },
                MenuCloses = false
            }
        };
    }
}

public class MeetingMainMenu : Menu
{
    private static bool CreateMeeting()
    {
        new MeetingEditingMenu(MeetingCollection.CreateMeeting(new Meeting())).Serve();

        return true;
    }

    public MeetingMainMenu()
    {
        Headline = () => $"Meetings Count: {MeetingCollection.Meetings.Count}";

        Options = new List<MenuOption>
        {
            new(() => "Finish Editing"),
            new(() => "Create a New Meeting")
            {
                Action = CreateMeeting,
                MenuCloses = false
            },
            new(() => "Show Meetings")
            {
                Action = () =>
                {
                    new MeetingSelectionMenu().Serve();
                    return true;
                },
                MenuCloses = false
            }
        };
    }
}