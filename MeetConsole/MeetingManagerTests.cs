using NUnit.Framework;

namespace MeetConsole;

internal class MeetingManagerTests
{
    private MeetingManager _manager = null!;
    private const string MeetingName = "jake meeting";

    [SetUp]
    public void SetUp()
    {
        _manager = new MeetingManager();
        var meeting = new Meeting(MeetingName, "my meeting", "jake", MeetingCategory.Hub, MeetingType.Live,
            new DateTime(1900, 1, 1), new DateTime(2000, 1, 1));
        Assert.IsTrue(_manager.TryAddMeeting(meeting));
    }

    [Test]
    public void TestAddMeetingDuplicateName()
    {
        // ReSharper disable once StringLiteralTypo
        var meeting = new Meeting("jaKE mEEting", "something else", "jack", MeetingCategory.Hub, MeetingType.Live,
            new DateTime(1900, 1, 1), new DateTime(2000, 1, 1));
        Assert.IsFalse(_manager.TryAddMeeting(meeting));
    }

    [Test]
    public void TestAddAttendeeCorrect()
    {
        var attendee = new Attendee("tom", new DateTime(1990, 1, 1));
        Assert.IsTrue(_manager.TryAddAttendeeToMeeting(MeetingName, attendee));
    }

    [Test]
    public void TestAddAttendeeWrongDate()
    {
        var attendee = new Attendee("tom", new DateTime(2000, 1, 2));
        Assert.IsFalse(_manager.TryAddAttendeeToMeeting(MeetingName, attendee));
    }

    [Test]
    public void TestAddAttendeeDuplicate()
    {
        var attendee = new Attendee("tom", new DateTime(1990, 1, 1));
        Assert.IsTrue(_manager.TryAddAttendeeToMeeting(MeetingName, attendee));
        Assert.IsFalse(_manager.TryAddAttendeeToMeeting(MeetingName, attendee));
    }

    [Test]
    public void TestAddAttendeeIntersectCorrect()
    {
        var attendee = new Attendee("tom", new DateTime(1910, 1, 1));
        Assert.IsTrue(_manager.TryAddAttendeeToMeeting(MeetingName, attendee));

        const string newMeetingName = "john meeting";
        var meeting = new Meeting("john meeting", "my meeting", "jake", MeetingCategory.Hub, MeetingType.Live,
            new DateTime(2010, 1, 1), new DateTime(2020, 1, 1));
        Assert.IsTrue(_manager.TryAddMeeting(meeting));

        attendee = new Attendee("tom", new DateTime(2011, 1, 1));
        Assert.IsTrue(_manager.TryAddAttendeeToMeeting(newMeetingName, attendee));
        Assert.IsFalse(_manager.HasIntersectingMeetings(newMeetingName, attendee));
    }

    [Test]
    public void TestAddAttendeeIntersectCorrect2()
    {
        var attendee = new Attendee("tom", new DateTime(1960, 1, 1));
        Assert.IsTrue(_manager.TryAddAttendeeToMeeting(MeetingName, attendee));

        const string newMeetingName = "john meeting";
        var meeting = new Meeting("john meeting", "my meeting", "jake", MeetingCategory.Hub, MeetingType.Live,
            new DateTime(1910, 1, 1), new DateTime(1950, 1, 1));
        Assert.IsTrue(_manager.TryAddMeeting(meeting));

        attendee = new Attendee("tom", new DateTime(1911, 1, 1));
        Assert.IsTrue(_manager.TryAddAttendeeToMeeting(newMeetingName, attendee));
        Assert.IsFalse(_manager.HasIntersectingMeetings(newMeetingName, attendee));
    }

    [Test]
    public void TestAddAttendeeIntersectIncorrect()
    {
        var attendee = new Attendee("tom", new DateTime(1940, 1, 1));
        Assert.IsTrue(_manager.TryAddAttendeeToMeeting(MeetingName, attendee));

        const string newMeetingName = "john meeting";
        var meeting = new Meeting(newMeetingName, "my meeting", "john", MeetingCategory.Hub, MeetingType.Live,
            new DateTime(1900, 1, 1), new DateTime(1950, 1, 1));
        Assert.IsTrue(_manager.TryAddMeeting(meeting));

        attendee = new Attendee("tom", new DateTime(1901, 1, 1));
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
        var attendee = new Attendee("tom", new DateTime(1940, 1, 1));
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
        var attendee = new Attendee("tom", new DateTime(1940, 1, 1));
        Assert.IsTrue(_manager.TryAddAttendeeToMeeting(MeetingName, attendee));
        Assert.IsTrue(_manager.TryRemoveAttendeeFromMeeting(MeetingName, "tom"));
    }

    [Test]
    public void TestRemoveAttendeeInvalidPerson()
    {
        var attendee = new Attendee("tom", new DateTime(1940, 1, 1));
        Assert.IsTrue(_manager.TryAddAttendeeToMeeting(MeetingName, attendee));
        Assert.IsFalse(_manager.TryRemoveAttendeeFromMeeting(MeetingName, "luke"));
    }

    [Test]
    public void TestRemoveResponsiblePerson()
    {
        var attendee = new Attendee("tom", new DateTime(1940, 1, 1));
        Assert.IsTrue(_manager.TryAddAttendeeToMeeting(MeetingName, attendee));
        Assert.IsFalse(_manager.TryRemoveAttendeeFromMeeting(MeetingName, "jake"));
    }
}