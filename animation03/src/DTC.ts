module MyGame {
    export class DTC {
        public numSkyTypes:number = 14;
        public numCloudTypes:number = 16;

        public truncateString(str:string, nChars:number):string {
            var result = "";
            var suffix = "..."
            
            if(str.length <= nChars) {
                result = str;
            }
            else {
                var strTruncated = str.substring(0, nChars - suffix.length);
                result = strTruncated + suffix;
            }
    
            return result;
        }
       
        public doubleDigit(n:number):string {
            var strResult = "";

            if(n < 100 && n > 0) {
                if(n < 10) 
                    strResult = "0" + n;
                else
                    strResult = n.toString();
            }

            return strResult;
        }
      
    }
}
