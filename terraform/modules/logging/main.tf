resource "aws_cloudwatch_log_group" "this" {
  name              = "/ecs/notes-api"
  retention_in_days = var.log_retention_days
}