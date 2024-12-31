namespace Selah.Core.Constants;

public static class SqlQueries
{
    public const string InsertIntoAccount =
        @"INSERT INTO Account (original_insert, last_update, id, app_last_changed_by, date_created, account_name) VALUES(@original_insert, @last_update, @id, @app_last_changed_by,  @date_created, @account_name)";

    public const string InsertIntoAppUser =
        @"INSERT INTO app_user(original_insert, last_update, app_last_changed_by,  id, account_id, created_date, encrypted_email, username, password,
                     encrypted_name, encrypted_phone, last_login, last_login_ip, phone_verified, email_verified) 
        VALUES(@original_insert, @last_update, @app_last_changed_by, @id,  @account_id, @created_date, 
               @encrypted_email, @username, @password, @encrypted_name, 
               @encrypted_phone, @last_login, 
               @last_login_ip, @phone_verified, @email_verified)";

    public const string GetUserById = "SELECT * FROM app_user WHERE id=@id";
}