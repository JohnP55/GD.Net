using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD
{
    public enum Status
    {
        Success,
        Error
    }

    public class GDServerResponse : GDSerializedObject
    {
        public const char DEFAULT_SEPARATOR = ':';
        public Status Status { get; } = Status.Success;
        public GDServerResponse(string response, char separator = DEFAULT_SEPARATOR) : base(response, separator)
        {
            if (response == "-1")
            {
                Status = Status.Error;
                return;
            }
        }
        public GDServerResponse(GDServerResponse response) : base(response)
        {
            Status = response.Status;
        }
    }
    public class GDIntegerResponse : GDServerResponse
    {
        public int Result { get; }
        public GDIntegerResponse(GDServerResponse response) : base(response)
        {
            Result = Convert.ToInt32(GetInt(0));
        }
    }
    public class GDUserResponse : GDServerResponse
    {
        public enum MessageStateValues
        {
            All,
            FriendsOnly,
            None
        }
        public enum FriendRequestStateValues
        {
            All,
            None
        }
        public enum FriendStateValues
        {
            None,
            IsFriends,
            RequestSend = 3,
            RequestReceived
        }
        public enum ModLevelValues
        {
            NoMod,
            Mod,
            ElderMod
        }
        public enum CommentHistoryStateValues
        {
            All,
            FriendsOnly,
            None
        }
        public string Username { get; }
        public int UserID { get; }
        public int Stars { get; }
        public int Demons { get; }
        public int Ranking { get; }
        public int AccountHighlight { get; }
        public int CreatorPoints { get; }
        public int IconID { get; }
        public int PlayerColor1 { get; }
        public int PlayerColor2 { get; }
        public int SecretCoins { get; }
        public int IconType { get; }
        public int Special { get; }
        public int AccountID { get; }
        public int UserCoins { get; }
        public MessageStateValues MessageState { get; }
        public FriendRequestStateValues FriendRequestState { get; }
        public string YouTube { get; }
        public int AccIcon { get; }
        public int AccShip { get; }
        public int AccBall { get; }
        public int AccBird { get; }
        public int AccDart { get; }
        public int AccRobot { get; }
        public int AccStreak { get; }
        public int AccGlow { get; }
        public bool IsRegistered { get; }
        public int GlobalRank { get; }
        public FriendStateValues FriendState { get; }
        public int UnreadMessages { get; }
        public int FriendRequests { get; }
        public int NewFriends { get; }
        public bool IsFriendRequestNew { get; }
        public string TimeSinceLevelScore { get; }
        public int AccSpider { get; }
        public string Twitter { get; }
        public string Twitch { get; }
        public int Diamonds { get; }
        public int AccExplosion { get; }
        public ModLevelValues ModLevel { get; } 
        public CommentHistoryStateValues CommentHistoryState { get; }
        public GDUserResponse(GDServerResponse response) : base(response)
        {
            Username = GetString(1);
            UserID = GetInt(2);
            Stars = GetInt(3);
            Demons = GetInt(4);
            Ranking = GetInt(6);
            AccountHighlight = GetInt(7);
            CreatorPoints = GetInt(8);
            IconID = GetInt(9);
            PlayerColor1 = GetInt(10);
            PlayerColor2 = GetInt(11);
            SecretCoins = GetInt(13);
            IconType = GetInt(14);
            Special = GetInt(15);
            AccountID = GetInt(16);
            UserCoins = GetInt(17);
            MessageState = (MessageStateValues)GetInt(18);
            FriendRequestState = (FriendRequestStateValues)GetInt(19);
            YouTube = GetString(20);
            AccIcon = GetInt(21);
            AccShip = GetInt(22);
            AccBall = GetInt(23);
            AccBird = GetInt(24);
            AccDart = GetInt(25);
            AccRobot = GetInt(26);
            AccStreak = GetInt(27);
            AccGlow = GetInt(28);
            IsRegistered = GetBoolean(29);
            GlobalRank = GetInt(30);
            FriendState = (FriendStateValues)GetInt(31);
            UnreadMessages = GetInt(38);
            FriendRequests = GetInt(39);
            NewFriends = GetInt(40);
            IsFriendRequestNew = GetBoolean(41);
            TimeSinceLevelScore = GetString(42);
            AccSpider = GetInt(43);
            Twitter = GetString(44);
            Twitch = GetString(45);
            Twitch = GetString(46);
            Diamonds = GetInt(48);
            ModLevel = (ModLevelValues)GetInt(49);
            CommentHistoryState = (CommentHistoryStateValues)GetInt(50);
        }
    }
    public class LevelDoesNotExistException : Exception
    {
        public LevelDoesNotExistException() : base("The level does not exist.") { }
    }
    public class GDLevelResponse : GDServerResponse
    {
        public int Id { get; }
        public string Name { get; }
        public string Description { get; }
        public string LevelString { get; }
        public int Version { get; }
        public int CreatorUserId { get; }
        public int Downloads { get; }
        public int OfficialSong { get; }
        public int GameVersion { get; }
        public int Likes { get; }
        public int Length { get; }
        public string RecordString { get; }
        public string Password { get; }
        public string UploadDate { get; }
        public string UpdateDate { get; }
        public int CopiedId { get; }
        public bool TwoPlayer { get; }
        public int CustomSongId { get; }
        public string ExtraString { get; }

        public GDLevelResponse(GDServerResponse response) : base(response)
        {
            if (response.Status == Status.Error) throw new LevelDoesNotExistException();
            Id = GetInt(1);
            Name = GetString(2);
            Description = GetString(3).Base64Decode();
            LevelString = GetString(4);
            Version = GetInt(5);
            CreatorUserId = GetInt(6);
            Downloads = GetInt(10);
            OfficialSong = GetInt(12);
            GameVersion = GetInt(13);
            Likes = GetInt(14);
            Length = GetInt(15);
            RecordString = GetString(26);

            Password = GetString(27);
            if (Password != "0")
                Password = Utils.UndoXorBase64(Password, "26364")[1..];

            UploadDate = GetString(28);
            UpdateDate = GetString(29);
            CopiedId = GetInt(30);
            TwoPlayer = GetBoolean(31);
            CustomSongId = GetInt(35);
            ExtraString = GetString(36);
            // Todo
        }
    }
}
