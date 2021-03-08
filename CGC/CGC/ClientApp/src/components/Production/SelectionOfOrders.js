import React, { Component } from 'react';
import { OrdersTable } from './OrdersTable';
import Sidebar from '../Sidebar';
import './SelectionOfOrders.css'
import ClipLoader from "react-spinners/ClipLoader";
import { MDBDataTableV5 } from 'mdbreact';
import { Link } from 'react-router-dom';



export class SelectionOfOrders extends Component {
    displayName = SelectionOfOrders.name;
    constructor(props) {
        super(props);
        this.state = {
            table: {
                columns: [],
                rows: [],
            },
            isLoading: true, 
        };
    }
    componentDidMount() {
        var table2 = [];
        console.log('start')
        console.log(new Date())
        console.log(this.state.isLoading)
        const receiver = {
            user: {
                company: sessionStorage.getItem('company'),
            }
        }
        fetch(`api/Cut/Return_Orders_To_Cut`, {
            method: "post",
            body: JSON.stringify(receiver),
            headers: {
                'Authorization': 'Bearer ' + sessionStorage.getItem('token'),
                'Content-Type': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {

                for (var i = 0; i < json.length; i++) {
                    table2.push({
                        number: json[i].id_Order,
                        //status: json[i].status,
                        klient: json[i].owner,
                        x: json[i].stan,
                        priority: json[i].priority,
                        deadline: json[i].deadline,
                        items: json[i].items,
                        choose: <Link to="/ready_packages"> <button className="success_t" id={i}
                            onClick={(e) => {
                                //this.chooseOrder( table2[e.target.id].number);
                                sessionStorage.setItem('orderId2', table2[e.target.id].number)

                            }
                            } > Select </button></Link>



                    })
                };
                this.setState({
                    table: {
                        columns: [
                            {
                                label: 'No.',
                                field: 'number',
                                sort: 'asc',
                                width: 30
                            },
                            {
                                label: 'Client',
                                field: 'klient',
                                sort: 'asc',
                                width: 150
                            },
                            {
                                label: 'X/Y/Z',
                                field: 'x',
                                sort: 'asc',
                                width: 150
                            },

                            {
                                label: 'Priority',
                                field: 'priority',
                                sort: 'asc',
                                width: 30
                            },
                            {
                                label: 'Deadline',
                                field: 'deadline',
                                sort: 'asc',
                                width: 30
                            },
                            /*{
                                label: 'Status',
                                field: 'status',
                                sort: 'asc',
                                width: 30
                            },*/
                            {
                                label: '',
                                field: 'choose',
                                width: 30
                            }
                        ],
                        rows: table2
                    },

                });
                console.log('koniec')
                console.log(new Date())
                this.setState({
                    isLoading: false
                })
                console.log(this.state.isLoading)
            })
    };
    chooseOrder(id) {
        const receiver = {
            order: {
                id_order: id
            },
            user: {
                company: sessionStorage.getItem('company'),
            }
        }

        fetch(`api/Cut/Return_Package_To_Cut`, {
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
        //   .then(json => this.props.history.push('/ready_packages'))



    }
    table() {
        return (
            <MDBDataTableV5


                data={this.state.table}
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
                className="table_corection"


            />

        )
    }


    homePage = (event) => {
        this.props.history.push('/home')
    }

    goback = (event) => {
        this.props.history.push('/')
    }
    goback2 = (event) => {
        this.props.history.push('/home')
    }

    render() {
        let table = this.table();
        if (sessionStorage.getItem('valid') === '') {
            if (this.state.isLoading === true) {
                return (
                    <ClipLoader loading={this.state.isLoading} size={150} />
                )
            } else {
                return (
                    <div>
                        <div className="phone">
                            <h1>No access on the phone</h1>
                        </div>
                        <div className="aaaaaaaa" >
                            <div className="HomePageFail">
                                <h1>Log in to have access!</h1>
                                <button type="submit" className="success_login2" onClick={this.goback} >Log in</button>
                            </div>
                        </div>
                    </div>
                );
            }
        }
        else if (this.state.isLoading === true) {
                return (
                            <ClipLoader loading={this.state.isLoading} size={150} />
                )
            }
            else {
                return (
                    <div>
                        <div className="phone">
                            <h1>No access on the phone</h1>
                        </div><div className="aaaaaaaa" >


                            <div className="SelectionOfOrders">
                                <Sidebar />
                                <div className="title">
                                    <h1 className="titletext">Select order</h1>
                                </div>
                                <div className="selection_of_orders_conteiner">


                                    <div>
                                        {table}
                                    </div>

                                </div>


                            </div>
                        </div>
                    </div>
                );
            }
        
    }

    
}