namespace Commands.Impls
{
    public static class MinIOUtil
    {
        public static string GetBucketName(string prefix, string md5) 
        {
           return $"{prefix}-{md5[..2]}";
        }
        
        public static string GetFileBucketName(string md5)
        {
            return GetBucketName("file", md5);
        }
    }
}