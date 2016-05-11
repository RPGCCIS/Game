using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fragments
{
    #region Flags
    [Flags]
    public enum ProgressFlags
    {
        FirstGateUnlocked = 1 << 0,
        TalkedWithElder = 1 << 1, //1
        SecondFragment = 1 << 2, //2
        ThirdFragment = 1 << 3, //4
        FourthFragment = 1 << 4, //8
        FifthFragment = 1 << 5, //16
        SixthFragment = 1 << 6 //32.......
    }
    #endregion

    class Progress
    {
        #region Singleton
        private static Progress instance;

        public static Progress Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Progress();
                }

                return instance;
            }
            set
            {
                instance = value;
            }
        }
        #endregion

        #region Variables
        private ProgressFlags flags;
        private int fragments;

        #endregion

        #region Properties
        public ProgressFlags Flags
        {
            get { return flags; }
            set { flags = value; }
        }

        public int Fragments
        {
            get { return fragments; }
            set { fragments = value; }
        }
        #endregion

        //Constructor
        public Progress()
        {
            flags = 0;
            fragments = 0;
        }

        //Methods
        public void SetProgress(ProgressFlags flag)
        {
            if (flags != (flags | flag))
            {
                flags |= flag;
            }
        }

        public override string ToString()
        {
            string value = "" + (int)flags
                + "\n" + fragments;

            return value;
        }
    }
}
