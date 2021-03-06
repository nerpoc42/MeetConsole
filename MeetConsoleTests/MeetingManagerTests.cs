using System;
using System.IO;
using NUnit.Framework;
using MeetConsole.Models;
using MeetConsole.Services;

namespace MeetConsoleTests;

public class MeetingManagerTests
{
    private MeetingManager _manager = null!;
    private const string MeetingName = "jake meeting";

    [SetUp]
    public void SetUp()
    {
        const string fileName = "test_meetings.json";
        // Ensure data from previous tests are not loaded.
        File.Create(fileName).Close(); 
        _manager = new MeetingManager(new MeetingRepository(fileName));
        
        var meeting = new Meeting
        {
            Name = MeetingName,
            Description = "my meeting",
            ResponsiblePerson = "jake",
            MeetingCategory = MeetingCategory.Hub,
            MeetingType = MeetingType.Live,
            StartDate = new DateTime(1900, 1, 1),
            EndDate = new DateTime(2000, 1, 1)
        };

        _manager.TryAddMeeting(meeting);
    }

    [Test]
    public void TestAddMeetingDuplicateName()
    {
        // ReSharper disable once StringLiteralTypo
        var meeting = new Meeting
        {
            Name = "jaKE mEEting",
            Description = "something else",
            ResponsiblePerson = "jack",
            MeetingCategory = MeetingCategory.Hub,
            MeetingType = MeetingType.Live,
            StartDate = new DateTime(1900, 1, 1),
            EndDate = new DateTime(2000, 1, 1)
        };
        Assert.IsFalse(_manager.TryAddMeeting(meeting));
    }

    [Test]
    public void TestAddAttendeeCorrect()
    {
        var attendee = new Attendee
        {
            Name = "tom",
            Date = new DateTime(1990, 1, 1)
        };
        Assert.IsTrue(_manager.TryAddAttendeeToMeeting(MeetingName, attendee));
    }

    [Test]
    public void TestAddAttendeeWrongDate()
    {
        var attendee = new Attendee
        {
            Name = "tom",
            Date = new DateTime(2000, 1, 2)
        };
        Assert.IsFalse(_manager.TryAddAttendeeToMeeting(MeetingName, attendee));
    }

    [Test]
    public void TestAddAttendeeDuplicate()
    {
        var attendee = new Attendee
        {
            Name = "tom",
            Date = new DateTime(1990, 1, 1)
        };
        Assert.IsTrue(_manager.TryAddAttendeeToMeeting(MeetingName, attendee));
        Assert.IsFalse(_manager.TryAddAttendeeToMeeting(MeetingName, attendee));
    }

    [Test]
    public void TestAddAttendeeIntersectCorrect()
    {
        var attendee = new Attendee
        {
            Name = "tom",
            Date = new DateTime(1910, 1, 1)
        };
        Assert.IsTrue(_manager.TryAddAttendeeToMeeting(MeetingName, attendee));

        const string newMeetingName = "john meeting";
        var meeting = new Meeting
        {
            Name = "john meeting",
            Description = "my meeting",
            ResponsiblePerson = "jake",
            MeetingCategory = MeetingCategory.Hub,
            MeetingType = MeetingType.Live,
            StartDate = new DateTime(2010, 1, 1),
            EndDate = new DateTime(2020, 1, 1)
        };
        Assert.IsTrue(_manager.TryAddMeeting(meeting));

        attendee = new Attendee
        {
            Name = "tom",
            Date = new DateTime(2011, 1, 1)
        };
        Assert.IsTrue(_manager.TryAddAttendeeToMeeting(newMeetingName, attendee));
        Assert.IsFalse(_manager.HasIntersectingMeetings(newMeetingName, attendee));
    }

    [Test]
    public void TestAddAttendeeIntersectCorrect2()
    {
        var attendee = new Attendee
        {
            Name = "tom",
            Date = new DateTime(1960, 1, 1)
        };
        Assert.IsTrue(_manager.TryAddAttendeeToMeeting(MeetingName, attendee));

        const string newMeetingName = "john meeting";
        var meeting = new Meeting
        {
            Name = "john meeting",
            Description = "my meeting",
            ResponsiblePerson = "jake",
            MeetingCategory = MeetingCategory.Hub,
            MeetingType = MeetingType.Live,
            StartDate = new DateTime(1910, 1, 1),
            EndDate = new DateTime(1950, 1, 1)
        };

        Assert.IsTrue(_manager.TryAddMeeting(meeting));

        attendee = new Attendee
        {
            Name = "tom",
            Date = new DateTime(1911, 1, 1)
        };
        Assert.IsTrue(_manager.TryAddAttendeeToMeeting(newMeetingName, attendee));
        Assert.IsFalse(_manager.HasIntersectingMeetings(newMeetingName, attendee));
    }

    [Test]
    public void TestAddAttendeeIntersectIncorrect()
    {
        var attendee = new Attendee
        {
            Name = "tom", 
            Date = new DateTime(1940, 1, 1)
        };
        Assert.IsTrue(_manager.TryAddAttendeeToMeeting(MeetingName, attendee));

        const string newMeetingName = "john meeting";
        var meeting = new Meeting
        {
            Name = newMeetingName,
            Description = "my meeting",
            ResponsiblePerson = "john",
            MeetingCategory = MeetingCategory.Hub,
            MeetingType = MeetingType.Live,
            StartDate = new DateTime(1900, 1, 1),
            EndDate = new DateTime(1950, 1, 1)
        };

        Assert.IsTrue(_manager.TryAddMeeting(meeting));

        attendee = new Attendee
        {
            Name = "tom",
            Date = new DateTime(1901, 1, 1)
        };
        Assert.IsTrue(_manager.TryAddAttendeeToMeeting(newMeetingName, attendee));
        Assert.IsTrue(_manager.HasIntersectingMeetings(newMeetingName, attendee));
    }

    [Test]
    public void TestRemoveMeetingResponsiblePerson()
    {
        Assert.IsFalse(_manager.TryRemoveMeeting(MeetingName, "tom"));
    }

    [Test]
    public void TestRemoveMeetingNotResponsiblePerson()
    {
        var attendee = new Attendee
        {
            Name = "tom",
            Date = new DateTime(1940, 1, 1)
        };
        Assert.IsTrue(_manager.TryAddAttendeeToMeeting(MeetingName, attendee));
        Assert.IsFalse(_manager.TryRemoveMeeting(MeetingName, "tom"));
    }

    [Test]
    public void TestRemoveMeetingInvalidPerson()
    {
        Assert.IsFalse(_manager.TryRemoveAttendeeFromMeeting(MeetingName, "luke"));
    }

    [Test]
    public void TestRemoveAttendeeCorrect()
    {
        var attendee = new Attendee
        {
            Name = "tom",
            Date = new DateTime(1940, 1, 1)
        };
        Assert.IsTrue(_manager.TryAddAttendeeToMeeting(MeetingName, attendee));
        Assert.IsTrue(_manager.TryRemoveAttendeeFromMeeting(MeetingName, "tom"));
    }

    [Test]
    public void TestRemoveAttendeeInvalidPerson()
    {
        var attendee = new Attendee
        {
            Name = "tom",
            Date = new DateTime(1940, 1, 1)
        };
        Assert.IsTrue(_manager.TryAddAttendeeToMeeting(MeetingName, attendee));
        Assert.IsFalse(_manager.TryRemoveAttendeeFromMeeting(MeetingName, "luke"));
    }

    [Test]
    public void TestRemoveResponsiblePerson()
    {
        var attendee = new Attendee
        {
            Name = "tom",
            Date = new DateTime(1940, 1, 1)
        };
        Assert.IsTrue(_manager.TryAddAttendeeToMeeting(MeetingName, attendee));
        Assert.IsFalse(_manager.TryRemoveAttendeeFromMeeting(MeetingName, "jake"));
    }
}