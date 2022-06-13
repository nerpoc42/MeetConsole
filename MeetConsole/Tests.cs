namespace MeetConsole;

using NUnit.Framework;

public class Tests
{
    private int _id;
    
    [SetUp]
    public void Setup()
    {
        var meeting = new Meeting
        {
            Category = MeetingCategory.Hub,
            Description = "test",
            EndDate = DateTime.Parse("2012/11/11"),
            StartDate = DateTime.Parse("2010/11/11"),
            Name = "Meeting",
            ResponsiblePerson = "Jack",
            Type = MeetingType.Live
        };
        _id = MeetingCollection.CreateMeeting(meeting);
    }

    [Test]
    public void TestAddToMeetingCorrect()
    {
        Assert.IsTrue(AttendeeCollection.AddToMeeting("Tom", _id, DateTime.Parse("2011/10/10")));
    }
    
    [Test]
    public void TestAddToMeetingWrongDate()
    {
        Assert.IsFalse(AttendeeCollection.AddToMeeting("Tom", _id, DateTime.Parse("2009/10/10")));
    }
    
    [Test]
    public void TestAddToMeetingDuplicate()
    {
        Assert.IsTrue(AttendeeCollection.AddToMeeting("Tom", _id, DateTime.Parse("2011/10/10")));
        Assert.IsFalse(AttendeeCollection.AddToMeeting("Tom", _id, DateTime.Parse("2011/11/11")));
    }

    [Test]
    public void RemoveFromMeetingCorrect()
    {
        Assert.IsTrue(AttendeeCollection.AddToMeeting("Luke", _id, DateTime.Parse("2011/10/10")));
        Assert.IsTrue(AttendeeCollection.RemoveFromMeeting("Luke", _id));
    }
    
    [Test]
    public void RemoveFromMeetingResponsiblePerson()
    {
        Assert.IsFalse(AttendeeCollection.RemoveFromMeeting("Jack", _id));
    }

    [Test]
    public void RemoveFromMeetingNotExisting()
    {
        Assert.IsFalse(AttendeeCollection.RemoveFromMeeting("Luke", _id));
    }

    [Test]
    public void RemoveMeetingCorrect()
    {
        Assert.IsTrue(MeetingCollection.RemoveMeeting(_id, "Jack"));
    }
    
    [Test]
    public void RemoveMeetingNotResponsiblePerson()
    {
        Assert.IsFalse(MeetingCollection.RemoveMeeting(_id, "Luke"));
    }
}
