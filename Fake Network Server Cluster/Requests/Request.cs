using System;

namespace Assi3 {
    class Request : Command {
        public string path;
        public int payload;

        public Request(string path, int payload) {
            this.path = path;
            this.payload = payload;
        } //end Request()

        /// <summary>Uses the Chain of Responsibility pattern to let the route handle the request</summary>
        public int Execute(Route route) {
            return route.HandleRequest(this);
        } //end Execute()
    } // end Request class
}
