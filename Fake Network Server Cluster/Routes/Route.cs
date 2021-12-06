using System;

namespace Assi3 {
    class Route {
        private string path;
        private Route next;
        
        public Route(string path, Route next = null) {
            this.path = path;
            this.next = next;
        } //end Route()

        /// <summary>Returns the appropriate number for its respective path by 
        ///             going down the Chain of Responsibility until either the paths match
        ///             or the chain ends</summary>
        public int HandleRequest(Request request) {
            if (path == request.path) {
                return Handler(request.payload);
            } else if (next != null) {
                return next.HandleRequest(request);
            }
            return 404;
        }

        /// <summary>Returns the appropriate number for its respective path</summary>
        public virtual int Handler(int arg) {
            return 404;
        }

    } //end Route class
}
