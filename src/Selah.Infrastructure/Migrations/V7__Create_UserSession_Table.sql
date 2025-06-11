CREATE TABLE user_session(
    id UUID PRIMARY KEY,
    user_id UUID,
    issued_at TIMESTAMPTZ,
    expires_at TIMESTAMPTZ
)INHERITS(base_audit_table);

CREATE INDEX user_session_user_id ON user_session(user_id);

/*
 ROLLBACK;
 DROP INDEX user_session_user_id;
 DELETE FROM flyway_schema_history where script = 'V7__Create_UserSession_Table.sql';
 DROP TABLE user_session CASCADE;
 */