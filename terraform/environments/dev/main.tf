module "network" {
  source   = "../../modules/network"
  vpc_cidr = var.vpc_cidr
}

module "logging" {
  source             = "../../modules/logging"
  log_retention_days = 14
}

module "storage" {
  source     = "../../modules/storage"
  table_name = "t_note"
}

module "iam" {
  source        = "../../modules/iam"
  table_arn     = module.storage.table_arn
  log_group_arn = module.logging.log_group_arn
}

module "compute" {
  source             = "../../modules/compute"
  vpc_id             = module.network.vpc_id
  public_subnets     = module.network.public_subnets
  private_subnets    = module.network.private_subnets
  ecs_task_role_arn  = module.iam.task_role_arn
  execution_role_arn = module.iam.execution_role_arn
  log_group_name     = module.logging.log_group_name
  image_uri          = var.image_uri
}