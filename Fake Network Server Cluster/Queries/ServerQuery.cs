using System;

namespace Assi3 {
    class ServerQuery : Query {
        /// <summary>Queries the server to see whether it is able to accept a request</summary>
        public bool CheckServer(Server server) {
            return server.IsAvailable();
        } //end CheckServer()
    } //end ServerQuery class
}
