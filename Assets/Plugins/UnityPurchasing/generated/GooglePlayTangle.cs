#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("fUCMRkw1Q/q3QW+veWxzP1XmjUdoRr1/wmU2ez7aD5yGuIHmqxsxXWOjlPJtckfkZDqcKudPP7U/J8ApqygmKRmrKCMrqygoKaZbAgO/8zAhueFMXLw2v8JfhepuwnahoGG2WcyoYcv1IRh0D2xXNVK8PnD2xj7jh5Vq4CvZUWuHY/1m+HJPdPhYUbfsn7VhpNK+v6/L54XQcSauGfrJruUmEDpGvXDk8RCSpIW8FHEb7/laTM6bBG8a9qD9r0DyZqEJ93+nnSF1ahyJaK+f/4E3qKpV2KoH2QYc6IJg451NSLm4BwbMXLv4JF74dppxGasoCxkkLyADr2Gv3iQoKCgsKSoQ5q9gB71i9s2nisgS7HOC9d/+6LhJMYAnWNaq6CsqKCko");
        private static int[] order = new int[] { 6,2,3,3,11,5,9,7,12,13,10,12,13,13,14 };
        private static int key = 41;

        public static byte[] Data() {
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
