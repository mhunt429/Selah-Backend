CREATE TABLE financial_accounts
(
    id      BIGSERIAL PRIMARY KEY,
    user_id UUID references app_user (id) ON DELETE SET NULL,
    connector_id BIGINT REFERENCES account_connector(id) ON DELETE SET NULL,
    external_id TEXT,
    current_balance DECIMAL,
    display_name TEXT,
    official_name TEXT,
    subtype TEXT,
    is_external_api_import BOOLEAN,
    last_api_import_time TIMESTAMPTZ,
    next_api_import_time TIMESTAMPTZ,
    import_job_lock TIMESTAMPTZ

)INHERITS(base_audit_table);

CREATE INDEX institution_indx ON financial_accounts(connector_id);
CREATE INDEX fa_user_id_indx ON financial_accounts(user_id);

/*
 ROLLBACK
  DELETE FROM schema_version where script = 'V2025.01.11.10.57.18__Add_Financial_Account_Table.sql';
 DROP INDEX institution_indx;
 DROP INDEX fa_user_id_indx;
 DROP TABLE financial_accounts;

 */