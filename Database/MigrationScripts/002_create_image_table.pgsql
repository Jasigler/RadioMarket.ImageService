CREATE TABLE "BucketReferences" (
    bucket_id uuid UNIQUE NOT NULL,
    bucket_item int UNIQUE NOT NULL, 
    CONSTRAINT "PK_bucketId" PRIMARY Key (bucket_id)
)