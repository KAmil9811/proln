import React, { Component } from 'react';
import './Acount.css';
import { UsersTable } from './UsersTable';



export class ControlPanel extends Component {
    displayName = ControlPanel.name;
    constructor(props) {
        super(props);
    }

    componentDidMount() {
        console.log(sessionStorage.getItem('manager'))
    }
    logOut = (event) => {
        event.preventDefault();
        sessionStorage.removeItem('email')
        sessionStorage.removeItem('login')
        sessionStorage.removeItem('name')
        sessionStorage.removeItem('password')
        sessionStorage.removeItem('surname')
        sessionStorage.removeItem('user')
        sessionStorage.removeItem('admin')
        sessionStorage.removeItem('superAdmin')
        sessionStorage.removeItem('manager')
        sessionStorage.removeItem('magazineManagement')
        sessionStorage.removeItem('machineManagement')
        sessionStorage.removeItem('orderManagement')
        sessionStorage.removeItem('cutManagement')
        this.props.history.push('/')
    }

    homePage = (event) => {
        this.props.history.push('/home')
    }

    addUser = (event) => {
        this.props.history.push('/add_acount')
    }

    changePassword = (event) => {
        this.props.history.push('/change_password')
    }
    changeEmail = (event) => {
        this.props.history.push('/change_email')
    }

    userChanging = (event) => {
        this.props.history.push('/user_change')
    }

    tableRender() {
        if (sessionStorage.getItem('admin') === 'true' || sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true') {
            return (<UsersTable />);
        } 
    }

    addAcouButton() {
        if (sessionStorage.getItem('admin') === 'true' || sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true') {
            return (<button className="add_user" onClick={this.addUser}>Dodaj Konto</button>);
        } 
    }

    

    
    editMachine = (event) => {
        this.props.history.push('/cutmachineedit')
    }

    editGlassColor = (event) => {
        this.props.history.push('/glassatibutes')
    }

    userHistory = (event) => {
        this.props.history.push('/user_history')
    }
    
    typeMachineAdd() {
        if (sessionStorage.getItem('admin') === 'true' || sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true') {
            return (
                <button type="button" className="types98" onClick={this.editMachine}>Typy maszyn</button>
            )
        }
    }
    colorAndTypeGlassEdit() {
        if (sessionStorage.getItem('admin') === 'true' || sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true') {
            return (
                <button type="button" className="types98" onClick={this.editGlassColor}>Zarządzanie szkłem</button>
            )
        }
    }

    usersHistoryTable() {
        return (
            <button type="button" className="types98" onClick={this.userHistory}>Historia użytkowników</button>
            )
    }
    adminPermissionsRender() {
        if (sessionStorage.getItem('admin') === 'true') {
            return (<option>Admin</option>)
        }
    }
    userPermissionsRender() {
        if (sessionStorage.getItem('user') === 'true') {
            return (<option>Pracownik</option>)
        }
    }
    superAdminPermissionsRender() {
        if (sessionStorage.getItem('superAdmin') === 'true') {
            return (<option>Super admin</option>)
        }
    }
    managerPermissionsRender() {
        if (sessionStorage.getItem('manager') === 'true') {
            return (<option>Menedżer</option>)
        }
    }
    magazineManagerPermissionsRender() {
        if (sessionStorage.getItem('magazineManagement') === 'true') {
            return (<option>Magazynier</option>)
        }
    }
    machineManagerPermissionsRender() {
        if (sessionStorage.getItem('machineManagement') === 'true') {
            return (<option>Menedżer maszyn</option>)
        }
    }
    orderManagerPermissionsRender() {
        if (sessionStorage.getItem('orderManagement') === 'true') {
            return (<option>Menedżer zleceń</option>)
        }
    }
    cutManagerPermissionsRender() {
        if (sessionStorage.getItem('cutManagement') === 'true') {
            return (<option>Menedżer cięcia</option>)
        }
    }



    render() {
        let buttonAdd = this.addAcouButton();
        let xd = this.tableRender();
        let typeMachine = this.typeMachineAdd();
        let admin = this.adminPermissionsRender();
        let user = this.userPermissionsRender();
        let superAdmin = this.superAdminPermissionsRender();
        let manager = this.managerPermissionsRender();
        let magazineManagement = this.magazineManagerPermissionsRender();
        let orderManagement = this.orderManagerPermissionsRender();
        let machineManagement = this.machineManagerPermissionsRender();
        let cutManagement = this.cutManagerPermissionsRender();
        let colorGlassEdit = this.colorAndTypeGlassEdit();
        let userHistoryTable = this.usersHistoryTable();

        return ( 
            <div className="ControlPanel" >
                <div className="nav_cp">
                     <button className="home" onClick={this.homePage}>Strona główna</button>
                    <button className="log_out" onClick={this.logOut}>Wyloguj</button>
                </div>

                 <div className="conteiner_cp">
                        <div className="">
                            <form>
                                <div className="form-group">
                                    <label>Login: {sessionStorage.getItem('login')}</label>
                                </div>
                                <div className="form-group">
                                    <label>
                                        Hasło:
                                        <button type="button" className="password_change" onClick={this.changePassword}>Zmień hasło</button>
                                    </label>
                                </div>
                                <div className="form-group">
                                    <label>
                                        E-mail: {sessionStorage.getItem('email')}
                                        <button type="button" className="email_change" onClick={this.changeEmail}>Zmień email</button>
                                    </label>
                                </div>
                                <div className="form-group">
                                    <label>Uprawnienia:
                                    {admin}
                                    {user}
                                    {superAdmin}
                                    {manager}
                                    {magazineManagement}
                                    {orderManagement}
                                    {machineManagement}
                                    {cutManagement}
                                    </label> 
                                </div>
                            </form>
                        </div>
                        <div>
                            {typeMachine}
                        {colorGlassEdit}
                        {userHistoryTable}
                    </div>
                    {buttonAdd}
                        <div className="tableuser">
                        {xd}
                    </div>
                </div>
            </div>
            
    )}


}