﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel.Composition;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GreenField.ServiceCaller.LoginDefinitions;
using System.Reflection;

namespace GreenField.ServiceCaller
{
    /// <summary>
    /// Class for interacting with Service LoginOperations - implements ASP Membership Provider APIs through WCF
    /// </summary>
    [Export(typeof(IManageLogins))]
    public class ManageLogins : IManageLogins
    {
        #region Service Caller Method Definitions
        #region Membership
        /// <summary>
        /// Validate User credentials
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="callback">True/False</param>
        public void ValidateUser(string username, string password, Action<bool?> callback)
        {
            LoginDefinitions.LoginOperationsClient client = new LoginDefinitions.LoginOperationsClient();
            client.ValidateUserAsync(username, password, callback);
            client.ValidateUserCompleted += (se, e) =>
            {
                try
                {
                    if (callback != null)
                        callback(e.Result);
                }
                catch (TargetInvocationException ex)
                {
                    MessageBox.Show("Message: " + ex.Message + "\nStackTrace: " + ex.StackTrace, "Exception", MessageBoxButton.OK);
                }
            };
        }

        /// <summary>
        /// Create Membership User
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="passwordQuestion"></param>
        /// <param name="passwordAnswer"></param>
        /// <param name="isApproved"></param>
        /// <param name="callback">MembershipCreateStatus</param>
        public void CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, Action<string> callback)
        {
            LoginDefinitions.LoginOperationsClient client = new LoginDefinitions.LoginOperationsClient();
            client.CreateUserAsync(username, password, email, passwordQuestion, passwordAnswer, isApproved, callback);
            client.CreateUserCompleted += (se, e) =>
            {
                try
                {
                    if (callback != null)
                        callback(e.Result.ToString());
                }
                catch (TargetInvocationException ex)
                {
                    MessageBox.Show("Message: " + ex.Message + "\nStackTrace: " + ex.StackTrace, "Exception", MessageBoxButton.OK);
                }
            };
        }

        /// <summary>
        /// Change Password
        /// </summary>
        /// <param name="username"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <param name="callback">True/False</param>
        public void ChangePassword(string username, string oldPassword, string newPassword, Action<bool?> callback)
        {
            LoginDefinitions.LoginOperationsClient client = new LoginDefinitions.LoginOperationsClient();
            client.ChangePasswordAsync(username, oldPassword, newPassword, callback);
            client.ChangePasswordCompleted += (se, e) =>
            {
                try
                {
                    if (callback != null)
                        callback(e.Result);
                }
                catch (TargetInvocationException ex)
                {
                    MessageBox.Show("Message: " + ex.Message + "\nStackTrace: " + ex.StackTrace, "Exception", MessageBoxButton.OK);
                }
            };
        }

        /// <summary>
        /// Reset Password
        /// </summary>
        /// <param name="username"></param>
        /// <param name="answer"></param>
        /// <param name="callback">New Password</param>
        public void ResetPassword(string username, string answer, Action<string> callback)
        {
            LoginDefinitions.LoginOperationsClient client = new LoginDefinitions.LoginOperationsClient();
            client.ResetPasswordAsync(username, answer, callback);
            client.ResetPasswordCompleted += (se, e) =>
            {
                try
                {
                    if (callback != null)
                        callback(e.Result.ToString());
                }
                catch (TargetInvocationException ex)
                {
                    MessageBox.Show("Message: " + ex.Message + "\nStackTrace: " + ex.StackTrace, "Exception", MessageBoxButton.OK);
                }
            };
        }

        /// <summary>
        /// Update Approval Status for Membership User
        /// </summary>
        /// <param name="user"></param>
        /// <param name="callback">True/False</param>
        public void UpdateApprovalForUser(MembershipUserInfo user, Action<bool?> callback)
        {
            LoginDefinitions.LoginOperationsClient client = new LoginDefinitions.LoginOperationsClient();
            client.UpdateApprovalForUserAsync(user, callback);
            client.UpdateApprovalForUserCompleted += (se, e) =>
            {
                try
                {
                    if (callback != null)
                        callback(e.Result);
                }
                catch (TargetInvocationException ex)
                {
                    MessageBox.Show("Message: " + ex.Message + "\nStackTrace: " + ex.StackTrace, "Exception", MessageBoxButton.OK);
                }
            };

        }

        /// <summary>
        /// Update Approval Status for multiple Membership Users
        /// </summary>
        /// <param name="users"></param>
        /// <param name="callback">True/False</param>
        public void UpdateApprovalForUsers(ObservableCollection<MembershipUserInfo> users, Action<bool?> callback)
        {
            LoginDefinitions.LoginOperationsClient client = new LoginDefinitions.LoginOperationsClient();
            client.UpdateApprovalForUsersAsync(users, callback);
            client.UpdateApprovalForUsersCompleted += (se, e) =>
            {
                try
                {
                    if (callback != null)
                        callback(e.Result);
                }
                catch (TargetInvocationException ex)
                {
                    MessageBox.Show("Message: " + ex.Message + "\nStackTrace: " + ex.StackTrace, "Exception", MessageBoxButton.OK);
                }
            };
        }

        /// <summary>
        /// Unlock Membership  User
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="callback">True/False</param>
        public void UnlockUser(string userName, Action<bool?> callback)
        {
            LoginDefinitions.LoginOperationsClient client = new LoginDefinitions.LoginOperationsClient();
            client.UnlockUserAsync(userName);
            client.UnlockUserCompleted += (se, e) =>
            {
                try
                {
                    if (callback != null)
                        callback(e.Result);
                }
                catch (TargetInvocationException ex)
                {
                    MessageBox.Show("Message: " + ex.Message + "\nStackTrace: " + ex.StackTrace, "Exception", MessageBoxButton.OK);
                }
            };
        }

        /// <summary>
        /// Unlock multiple Membership  Users
        /// </summary>
        /// <param name="userNames"></param>
        /// <param name="callback">True/False</param>
        public void UnlockUsers(ObservableCollection<string> userNames, Action<bool?> callback)
        {
            LoginDefinitions.LoginOperationsClient client = new LoginDefinitions.LoginOperationsClient();
            client.UnlockUsersAsync(userNames);
            client.UnlockUsersCompleted += (se, e) =>
            {
                try
                {
                    if (callback != null)
                        callback(e.Result);
                }
                catch (TargetInvocationException ex)
                {
                    MessageBox.Show("Message: " + ex.Message + "\nStackTrace: " + ex.StackTrace, "Exception", MessageBoxButton.OK);
                }
            };
        }

        /// <summary>
        /// Get Membership User
        /// </summary>
        /// <param name="username"></param>
        /// <param name="userIsOnline"></param>
        /// <param name="callback">MembershipUser</param>
        public void GetUser(string username, bool userIsOnline, Action<MembershipUserInfo> callback)
        {
            LoginDefinitions.LoginOperationsClient client = new LoginDefinitions.LoginOperationsClient();
            client.GetUserAsync(username, userIsOnline);
            client.GetUserCompleted += (se, e) =>
            {
                try
                {
                    if (callback != null)
                        callback(e.Result);
                }
                catch (TargetInvocationException ex)
                {
                    MessageBox.Show("Message: " + ex.Message + "\nStackTrace: " + ex.StackTrace, "Exception", MessageBoxButton.OK);
                }
            };
        }

        /// <summary>
        /// Get all Membership Users
        /// </summary>
        /// <param name="callback">Users</param>
        public void GetAllUsers(Action<System.Collections.Generic.List<MembershipUserInfo>> callback)
        {
            LoginDefinitions.LoginOperationsClient client = new LoginDefinitions.LoginOperationsClient();
            client.GetAllUsersAsync();
            client.GetAllUsersCompleted += (se, e) =>
            {
                try
                {
                    if (callback != null)
                        callback(e.Result.ToList());
                }
                catch (TargetInvocationException ex)
                {
                    MessageBox.Show("Message: " + ex.Message + "\nStackTrace: " + ex.StackTrace, "Exception", MessageBoxButton.OK);
                }
            };
        }

        /// <summary>
        /// Delete Membership User
        /// </summary>
        /// <param name="username"></param>
        /// <param name="callback">True/False</param>
        public void DeleteUser(string username, Action<bool?> callback)
        {
            LoginDefinitions.LoginOperationsClient client = new LoginDefinitions.LoginOperationsClient();
            client.DeleteUserAsync(username);
            client.DeleteUserCompleted += (se, e) =>
            {
                try
                {
                    if (callback != null)
                        callback(e.Result);
                }
                catch (TargetInvocationException ex)
                {
                    MessageBox.Show("Message: " + ex.Message + "\nStackTrace: " + ex.StackTrace, "Exception", MessageBoxButton.OK);
                }
            };
        }

        /// <summary>
        /// Delete multiple Membership Users
        /// </summary>
        /// <param name="username"></param>
        /// <param name="callback">True/False</param>
        public void DeleteUsers(ObservableCollection<string> usernames, Action<bool?> callback)
        {
            LoginDefinitions.LoginOperationsClient client = new LoginDefinitions.LoginOperationsClient();
            client.DeleteUsersAsync(usernames);
            client.DeleteUsersCompleted += (se, e) =>
            {
                try
                {
                    if (callback != null)
                        callback(e.Result);
                }
                catch (TargetInvocationException ex)
                {
                    MessageBox.Show("Message: " + ex.Message + "\nStackTrace: " + ex.StackTrace, "Exception", MessageBoxButton.OK);
                }
            };
        }
        #endregion

        #region Roles
        /// <summary>
        /// Create Role
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="callback">True/False</param>
        public void CreateRole(string roleName, Action<bool?> callback)
        {
            LoginDefinitions.LoginOperationsClient client = new LoginDefinitions.LoginOperationsClient();
            client.CreateRoleAsync(roleName, callback);
            client.CreateRoleCompleted += (se, e) =>
            {
                try
                {
                    if (callback != null)
                        callback(e.Result);
                }
                catch (TargetInvocationException ex)
                {
                    MessageBox.Show("Message: " + ex.Message + "\nStackTrace: " + ex.StackTrace, "Exception", MessageBoxButton.OK);
                }
            };
        }

        /// <summary>
        /// Get all Roles
        /// </summary>
        /// <param name="callback">Roles</param>
        public void GetAllRoles(Action<System.Collections.Generic.List<string>> callback)
        {
            LoginDefinitions.LoginOperationsClient client = new LoginDefinitions.LoginOperationsClient();
            client.GetAllRolesAsync();
            client.GetAllRolesCompleted += (se, e) =>
            {
                try
                {
                    if (callback != null)
                        callback(e.Result.ToList());
                }
                catch (TargetInvocationException ex)
                {
                    MessageBox.Show("Message: " + ex.Message + "\nStackTrace: " + ex.StackTrace, "Exception", MessageBoxButton.OK);
                }
            };
        }

        /// <summary>
        /// Get Roles for Membership User
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="callback">Roles</param>
        public void GetRolesForUser(string userName, Action<List<string>> callback)
        {
            LoginDefinitions.LoginOperationsClient client = new LoginDefinitions.LoginOperationsClient();
            client.GetRolesForUserAsync(userName);
            client.GetRolesForUserCompleted += (se, e) =>
            {
                try
                {
                    if (callback != null)
                        callback(e.Result.ToList());
                }
                catch (TargetInvocationException ex)
                {
                    MessageBox.Show("Message: " + ex.Message + "\nStackTrace: " + ex.StackTrace, "Exception", MessageBoxButton.OK);
                }
            };
        }

        /// <summary>
        /// Remove Membership Users from Roles
        /// </summary>
        /// <param name="usernames"></param>
        /// <param name="roleNames"></param>
        /// <param name="callback">True/False</param>
        public void RemoveUsersFromRoles(ObservableCollection<string> usernames, ObservableCollection<string> roleNames, Action<bool?> callback)
        {
            LoginDefinitions.LoginOperationsClient client = new LoginDefinitions.LoginOperationsClient();
            client.RemoveUsersFromRolesAsync(usernames, roleNames);
            client.RemoveUsersFromRolesCompleted += (se, e) =>
            {
                try
                {
                    if (callback != null)
                        callback(e.Result);
                }
                catch (TargetInvocationException ex)
                {
                    MessageBox.Show("Message: " + ex.Message + "\nStackTrace: " + ex.StackTrace, "Exception", MessageBoxButton.OK);
                }
            };
        }

        /// <summary>
        /// Add Membership Users to Roles
        /// </summary>
        /// <param name="usernames"></param>
        /// <param name="roleNames"></param>
        /// <param name="callback"></param>
        public void AddUsersToRoles(ObservableCollection<string> usernames, ObservableCollection<string> roleNames, Action<bool?> callback)
        {
            LoginDefinitions.LoginOperationsClient client = new LoginDefinitions.LoginOperationsClient();
            client.AddUsersToRolesAsync(usernames, roleNames);
            client.AddUsersToRolesCompleted += (se, e) =>
            {
                try
                {
                    if (callback != null)
                        callback(e.Result);
                }
                catch (TargetInvocationException ex)
                {
                    MessageBox.Show("Message: " + ex.Message + "\nStackTrace: " + ex.StackTrace, "Exception", MessageBoxButton.OK);
                }
            };
        }

        /// <summary>
        /// Delete Role
        /// </summary>
        /// <param name="username"></param>
        /// <param name="throwOnPopulatedRole"></param>
        /// <param name="callback">True/False</param>
        public void DeleteRole(string username, bool throwOnPopulatedRole, Action<bool?> callback)
        {
            LoginDefinitions.LoginOperationsClient client = new LoginDefinitions.LoginOperationsClient();
            client.DeleteRoleAsync(username, throwOnPopulatedRole, callback);
            client.DeleteRoleCompleted += (se, e) =>
            {
                try
                {
                    if (callback != null)
                        callback(e.Result);
                }
                catch (TargetInvocationException ex)
                {
                    MessageBox.Show("Message: " + ex.Message + "\nStackTrace: " + ex.StackTrace, "Exception", MessageBoxButton.OK);
                }
            };
        }

        /// <summary>
        /// Update Membership User Roles
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="addRoleNames"></param>
        /// <param name="deleteRoleNames"></param>
        /// <param name="callback">True/False</param>
        public void UpdateUserRoles(string userName, ObservableCollection<string> addRoleNames, ObservableCollection<string> deleteRoleNames, Action<bool?> callback)
        {
            LoginDefinitions.LoginOperationsClient client = new LoginDefinitions.LoginOperationsClient();
            client.UpdateUserRolesAsync(userName, addRoleNames, deleteRoleNames);
            client.UpdateUserRolesCompleted += (se, e) =>
            {
                try
                {
                    if (callback != null)
                        callback(e.Result);
                }
                catch (TargetInvocationException ex)
                {
                    MessageBox.Show("Message: " + ex.Message + "\nStackTrace: " + ex.StackTrace, "Exception", MessageBoxButton.OK);
                }
            };
        }
        #endregion 
        #endregion        
    }
}
