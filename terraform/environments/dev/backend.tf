terraform {
  backend "s3" {
    bucket         = "terraform-state-example"
    key            = "dev/terraform.tfstate"
    region         = "ap-southeast-1"
    dynamodb_table = "terraform-locks"
    encrypt        = true
  }
}