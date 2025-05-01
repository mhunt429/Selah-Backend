CREATE TABLE financial_account
(
    id                     UUID Primary KEY,
    user_id                UUID REFERENCES app_user (id) ON DELETE SET NULL,
    connector_id           UUID,
    external_id            TEXT,
    current_balance        DECIMAL,
    account_mask           VARCHAR(16),
    display_name           TEXT,
    official_name          TEXT,
    subtype                TEXT,
    is_external_api_import BOOLEAN,
    last_api_sync_time     TIMESTAMPTZ
) INHERITS(base_audit_table);

CREATE INDEX fa_connectorId on financial_account (connector_id);

/*
 ROLLBACK DROP INDEX fa_connectorId;
DELETE FROM flyway_schema_history WHERE script = 'V6__Create_Financial_Account_Table.sql';
 DROP TABLE financial_account;
 */