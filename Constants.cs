namespace BlockPuzzle
{
    public static class Constants
    {
        //for save setting
        public const string PathOfSettingFile = "Settings.txt";

        //sefault settings 
        public const int DefaultDepth = 20;
        public const int DefaultWidth = 10;
        public const int DefaultSpeed = 150;
        public const bool ConfirmExit = true;
        public const bool DisableSound = false;

        //size of rectangle
        //public const int SizeInPixels = 15;
        public const int SizeInPixels = 40;

        // to  check name length
        public const string MessageAbouteIncorrectNameLength = "The name you entered is too long (maximum {0} characters)";
        public const int MaxLengthForName = 25;

        // to print records
        public const int MaxLengthForRecordString = 40;
        public const int SpacesLengthForRecordString = 2;
        public const int ScoreLengthForRecordString = 5;
        public const int PositionLengthForRecordString = 3;

        public const string TextForARecord = "{0:d2}.{1} {2} {3:d5}";
        public const char Separator = '_';

        public const int MaxRecords = 15;

        // to find interval by speed
        public const double MilisecondsInAMinute = 60000;

        // to increase score
        public const int PointsForARow = 10;
        public const int PointsForBigWin = 100;

        // to generate figures
        public const int GeneratedValueForS = 1;
        public const int GeneratedValueForZ = 2;
        public const int GeneratedValueForT = 3;
        public const int GeneratedValueForO = 4;
        public const int GeneratedValueForJ = 6;
        public const int GeneratedValueForL = 5;
        public const int GeneratedValueForI = 7;

        // to pause
        public const string StatusGamePause = "Pause";
        public const string StatusGamePlay = "Start";
        public const string TextForContinueGame = "Click OK to continue";

        // to exit game
        public const string ExitGameCaption = "Confirm";
        public const string ExitGameText = "Are you sure you want to exit the game?";
    }
}