CREATE TABLE transaction_import_job
(
    id      BIGSERIAL PRIMARY KEY,
    user_id      UUID   references app_user (id) ON DELETE SET NULL,
    connector_id BIGINT REFERENCES account_connector (id) ON DELETE SET NULL,
    job_status   VARCHAR(20), --Running, Success, Failed
    last_run     TIMESTAMPTZ,
    next_run     TIMESTAMPTZ,
    job_lock     TIMESTAMPTZ
);

CREATE INDEX tij_user_indx ON transaction_import_job(user_id)


/*
 ROLLBACK DELETE schema_version where script = 'V2025.01.11.23.14.29__Add_Transaction_Import_Job_Table.sql';
 DROP INDEX tij_user_indx
 DROP TABLE transaction_import_job
 */