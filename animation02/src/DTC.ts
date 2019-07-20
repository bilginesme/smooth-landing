module MyGame {
    export class DTC {
        public numSkyTypes:number = 13;
        public numSeaTypes:number = 3;
        public numCloudTypes:number = 16;
        public numStarfishLegs:number = 5;
        public numBoatTypes:number = 33;
        public numSubmarineTypes:number = 6;
        public numSubmarineSounds:number = 8;

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
