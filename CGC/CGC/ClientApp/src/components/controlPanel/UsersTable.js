import React, { Component} from 'react';
import { MDBDataTableV5  } from 'mdbreact';
import { Link } from 'react-router-dom';
import './Acount.css';
import './UserTable.css';
import { Route } from 'react-router-dom';
import Sidebar from '../Sidebar';



export class UsersTable extends Component {
    constructor(props) {
        super(props);
        this.state = {
            table333: {
                columns: [],
                rows: []
            },
        };
    }
    


    componentDidMount() {
        var table = [];
        var link = '';
        const model = {
            client_id: sessionStorage.getItem('token'),
        }
        if (sessionStorage.getItem('manager') === 'true') {
            link = 'api/Users/Return_All_Users'
        }
        else if (sessionStorage.getItem('superAdmin') === 'true') {
            link = 'api/Users/Return_All_SuperAdmin'
        }
        else {
            link = 'api/Users/Return_All_Admin'
        }
        fetch(`${link}`, {
            method: "get",
            /*body: JSON.stringify(
                model
            ),*/
            headers: {
                'Authorization': 'Bearer ' + sessionStorage.getItem('token'), 
                'Content-Type': 'application/json',
                'Accept': 'application/json',
            }
            })
            .then(res => res.json())
            .then(json => {
                console.log(json)
                for (var i = 0; i < json.length; i++) {
                    var deleted = '';
                    if (json[i].deleted === false) {
                        deleted = 'Active'
                    }
                    else {
                        deleted = 'Deleted'
                    }
                    table.push({
                        id: i + 1,
                        name: json[i].name,
                        login: json[i].login,
                        password: json[i].password,
                        secondName: json[i].surname,
                        email: json[i].email,
                        permissions: 'Employee',
                        deleted: deleted,
                        action: <Link to="/user_change"><button className="info_t" id={i}
                            onClick={
                                (e) => {
                                   
                                    sessionStorage.setItem('editPerm', table[e.target.id].permissions);
                                    sessionStorage.setItem('editLogin', table[e.target.id].login);
                                    sessionStorage.setItem('editPassword', table[e.target.id].password);
                                    sessionStorage.setItem('editName', table[e.target.id].name);
                                    sessionStorage.setItem('editSecondName', table[e.target.id].secondName);
                                    sessionStorage.setItem('editMail', table[e.target.id].email);
                                }
                            }>Edit</button></Link>,
                        del: <button className="danger_t" id={i} onClick={(e) => { this.delete(table[e.target.id].login, table[e.target.id].deleted) }}> Delete/Restore  </button> 
                    })
                };
               
                for (var k = 0; k < table.length; k++) {
                    if (json[k].admin === true) {
                        table[k].permissions = table[k].permissions +', '+ 'Admin'
                    }
                    if (json[k].super_Admin === true) {
                        table[k].permissions = table[k].permissions + ', ' + 'Super admin'
                    }
                    if (json[k].manager === true) {
                        table[k].permissions = table[k].permissions + ', ' + 'Manager'
                    }
                    if (json[k].magazine_management === true) {
                        table[k].permissions = table[k].permissions + ', ' + 'Magazine management'
                    }
                    if (json[k].machine_management === true) {
                        table[k].permissions = table[k].permissions + ', ' + 'Machine management'
                    }
                    if (json[k].order_management === true) {
                        table[k].permissions = table[k].permissions + ', ' + 'Order management'
                    }
                    if (json[k].cut_management === true) {
                        table[k].permissions = table[k].permissions + ', ' + 'Cut management'
                    }
                }
                this.setState({
                    table333: {
                        columns: [
                            {
                                label: 'Login',
                                field: 'login',
                                sort: 'asc',
                                width: 150
                            },
                            {
                                label: 'Name',
                                field: 'name',
                                sort: 'asc',
                                width: 250
                            },
                            {
                                label: 'Surname',
                                field: 'secondName',
                                sort: 'asc',
                                width: 200
                            },
                            {
                                label: 'Permissions',
                                field: 'permissions',
                                sort: 'asc',
                                width: 100
                            },
                            {
                                label: 'Edit',
                                field: 'action',
                                width: 100
                            },
                            {
                                label: 'Account status',
                                field: 'deleted',
                                width: 100
                            },
                            {
                                label: 'Delete',
                                field: 'del',
                                width: 100
                            }
                    ],
                        rows: table 
                    }
                });
            })
    };

    delete(user, deleted) {
        const receiver = {
            user: {
                login: user
            },
            admin: {
                login: sessionStorage.getItem('login')
            }
        }
        

        if (deleted === 'Active') {
            fetch(`api/Users/Remove_User_Admin`, {
                method: "post",
                body: JSON.stringify(receiver),
                headers: {
                    'Content-Type': 'application/json'
                }
            })

                .then(res => res.json())
                .then(json => {
                    
                    return (json);
                })
                .then(json => {
                    alert("You deleted user")
                })
                .then(json => {
                    window.location.reload();
                })
        }
        else {
            fetch(`api/Users/Restore_User_Admin`, {
                method: "post",
                body: JSON.stringify(receiver),
                headers: {
                    'Content-Type': 'application/json'
                }
            })

                .then(res => res.json())
                .then(json => {
                    
                    return (json);
                })
                .then(json => {
                    alert("You activated user")
                })
                .then(json => {
                    window.location.reload();
                })
        }
        

    }
 

    table() {  
        return (
            <MDBDataTableV5

                data={this.state.table333}
                hover
                entriesOptions={[10, 20, 50, 100]}
                entries={10}
                pagesAmount={10}
                searchTop
                materialSearch
                searchBottom={false}
                responsive
                bordered
                paginationLabel={["Previous", "Next"]}
                sortable
                // small
                theadTextWhite
                theadTextWhite
                className="table_corection_add_o"






            />
           
        )
    }

    render() {
        let xd = this.table();
        return (
            
            <div>
                {xd}
            </div>
            )
    }

}