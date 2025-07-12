CREATE TABLE financial_account
(
    id                     UUID Primary KEY,
    user_id                UUID REFERENCES app_user (id) ON DELETE SET NULL DEFERRABLE INITIALLY DEFERRED,
    connector_id           UUID REFERENCES account_connector (id) ON DELETE SET NULL DEFERRABLE INITIALLY DEFERRED,
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
CREATE INDEX fa_userId ON financial_account (user_id);
CREATE INDEX fa_externalId on financial_account(external_id);

/*
 ROLLBACK DROP INDEX fa_connectorId;
 DROP INDEX fa_userId;
DELETE FROM flyway_schema_history WHERE script = 'V6__Create_Financial_Account_Table.sql';
 DROP TABLE financial_account;
 */