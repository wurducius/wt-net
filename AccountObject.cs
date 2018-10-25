using System;
using System.Collections.Generic;
using System.Text;
using WindingTreeNet.Model;

namespace WindingTreeNet
{
    /*       {
  "wallet": {"version":3,"id":"7fe84016-4686-4622-97c9-dc7b47f5f5c6","address":"d037ab9025d43f60a31b32a82e10936f07484246","crypto":{"ciphertext":"ef9dcce915eeb0c4f7aa2bb16b9ae6ce5a4444b4ed8be45d94e6b7fe7f4f9b47","cipherparams":{"iv":"31b12ef1d308ea1edacc4ab00de80d55"},"cipher":"aes-128-ctr","kdf":"scrypt","kdfparams":{"dklen":32,"salt":"d06ccd5d9c5d75e1a66a81d2076628f5716a3161ca204d92d04a42c057562541","n":8192,"r":8,"p":1},"mac":"2c30bc373c19c5b41385b85ffde14b9ea9f0f609c7812a10fdcb0a565034d9db"}},
  "uploaders": {
    "root": {
      "swarm": {}
    }
  }
}*/

    public class AccountObject
    {
        public Wallet wallet;
        public Uploaders uploaders;

        public AccountObject(Wallet _wallet)
        {
            wallet = _wallet;

            uploaders = new Uploaders();
            uploaders.root = new Root();
            uploaders.root.swarm = new Swarm();
        }
    }
}
