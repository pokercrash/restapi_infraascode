variable "vpc_id" {}
variable "public_subnets" {}
variable "private_subnets" {}
variable "ecs_task_role_arn" {}
variable "execution_role_arn" {}
variable "log_group_name" {}

variable "image_uri" {
  type    = string
  default = "nginx:latest"
}