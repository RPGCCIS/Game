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
        SecondTown = 1 << 1, //1
        Flag3 = 1 << 2, //2
        Flag4 = 1 << 3, //4
        Flag5 = 1 << 4, //8
        Flag6 = 1 << 5, //16
        Flag7 = 1 << 6 //32.......
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

        //Variables
        private ProgressFlags flags;

        //Properties
        public ProgressFlags Flags
        {
            get { return flags; }
            set { flags = value; }
        }

        //Constructor
        public Progress()
        {
            flags = 0;
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
            string value = "" + (int)flags;
            return value;
        }
    }
}
