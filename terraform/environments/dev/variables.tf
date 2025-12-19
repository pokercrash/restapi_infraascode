variable "vpc_cidr" {
  type = string
}

variable "image_uri" {
  type        = string
  description = "Container image URI pushed by CI"
}