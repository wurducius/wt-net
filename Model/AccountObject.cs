using System;
using System.Collections.Generic;
using System.Text;

namespace WindingTreeNet.Model
{
    public class AccountObject
    {
        public Wallet wallet;
        public Uploaders uploaders;

        // Currently supports only swarm data source
        public AccountObject(Wallet _wallet)
        {
            wallet = _wallet;

            uploaders = new Uploaders();
            uploaders.root = new Root();
            uploaders.root.swarm = new Swarm();
        }
    }
}
