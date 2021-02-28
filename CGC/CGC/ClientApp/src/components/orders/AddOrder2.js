import React, { Component } from "react";
import { MDBDataTableV5 } from 'mdbreact';
import './AddOrder2.css'
import Sidebar from '../Sidebar';

export class AddOrderTwo extends Component {
    displayName = AddOrderTwo;
    constructor(props) {
        super(props);        
        this.state = {
            table: {
                columns: [
                    {
                        label: 'Length',
                        field: 'length',
                        sort: 'asc',
                        width: 150
                    },
                    {
                        label: 'Width',
                        field: 'width',
                        sort: 'asc',
                        width: 150
                    },
                    {
                        label: 'Thickness',
                        field: 'thickness',
                        sort: 'asc',
                        width: 150
                    },
                    {
                        label: 'Color',
                        field: 'color',
                        sort: 'asc',
                        width: 150
                    },
                    {
                        label: 'Type',
                        field: 'type',
                        sort: 'asc',
                        width: 150
                    },
                    {
                        label: 'Amount',
                        field: 'amount',
                        sort: 'asc',
                        width: 150
                    },
                ],
                rows: []
            },
            colors: [],
            type: [],
            check:  '',
            tabliastringow: [],
        }
    }


    componentDidMount() {
        var table2 = [];
        var table3 = [];
        fetch(`api/Magazine/Return_All_Colors`, {
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {
                
                for (var i = 0; i < json.length; i++) {
                    table2.push({
                        color: json[i],
                    })
                };
                this.setState({
                    colors: table2

                });
            })

        fetch(`api/Magazine/Return_All_Type`, {
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {
                
                for (var i = 0; i < json.length; i++) {
                    table3.push({
                        type: json[i],
                    })
                };
                this.setState({
                    type: table3

                });
            })
    }

    handleAddOrder = (event) => {
        event.preventDefault();
        const receiver = {
            order: {
                owner: sessionStorage.getItem('client'),
                priority: sessionStorage.getItem('prioryty'),
                deadline: sessionStorage.getItem('deadline'),
            },
            iteme: this.state.tabliastringow,
            user: {
                login: sessionStorage.getItem('login'),
            }
        }
        if (this.state.check === '') {
            alert('You did not add any item')
        }
        else {
            fetch(`api/Order/Add_Order`, {
                method: "post",
                body: JSON.stringify(receiver),
                headers: {
                    'Authorization': 'Bearer ' + sessionStorage.getItem('token'),
                    'Content-Type': 'application/json'
                }
            })
                .then(res => res.json())
                .then(json => {
                    this.props.history.push('/orderwarehouse')
                })
            
        }
    }
       

    goBack = (event) => {
        sessionStorage.removeItem('client'),
        sessionStorage.removeItem('prioryty'),
        sessionStorage.removeItem('deadline'),
        this.props.history.push('/addorderfirst')
    }

    cancelAdding = (event) => {
        sessionStorage.removeItem('client'),
        sessionStorage.removeItem('prioryty'),
        sessionStorage.removeItem('deadline'),
        this.props.history.push('/orderwarehouse')

    }

    addItem = (event, table) => {
        event.preventDefault();
        var table2 = [];
        if (this.refs.width.value === "" || this.refs.length.value === "" || this.refs.thickness.value === "" || this.refs.color.value === "" || this.refs.amount.value === "" || this.refs.type.value === "" ) {
            alert("Wprowadź dane")
        }
        else {


        this.setState({
            table: {
                columns: [
                    {
                        label: 'Length',
                        field: 'length',
                        sort: 'asc',
                        width: 150
                    },
                    {
                        label: 'Width',
                        field: 'width',
                        sort: 'asc',
                        width: 150
                    },
                    {
                        label: 'Thickness',
                        field: 'thickness',
                        sort: 'asc',
                        width: 150
                    },
                    {
                        label: 'Color',
                        field: 'color',
                        sort: 'asc',
                        width: 150
                    },
                    {
                        label: 'Type',
                        field: 'type',
                        sort: 'asc',
                        width: 150
                    },
                    {
                        label: 'Amount',
                        field: 'amount',
                        sort: 'asc',
                        width: 150
                    },
                    {
                        label: 'Delete',
                        field: 'action',
                        sort: 'asc',
                        width: 150
                    }
                ],
                rows: this.state.table.rows.concat([{
                    width:  this.refs.width.value,
                    length: this.refs.length.value,
                    thickness: this.refs.thickness.value,
                    color: this.refs.color.value,
                    amount: this.refs.amount.value,
                    type: this.refs.type.value,
                   // delete: <button className="danger_t" id={i} onClick={(e) => { this.delete(table2[e.target.id].number, table2[e.target.id].deleted) }}> Usuń  </button>,
                    shape: 'rectangle',
                }]),
            },
            check: 'ok',
            tabliastringow: this.state.tabliastringow.concat(this.refs.width.value, this.refs.length.value, this.refs.thickness.value, this.refs.color.value, this.refs.amount.value, this.refs.type.value,)
        })
            
        }
    }

    colorsSelector = (event) => {
        var tab = []
        for (var i = 0; i < this.state.colors.length; i++) {

            tab.push(< option value={this.state.colors[i].color} > {this.state.colors[i].color}</option >)


        }
        return (tab)
    }
    typeSelector = (event) => {
        var tab = []
        for (var i = 0; i < this.state.type.length; i++) {

            tab.push(< option value={this.state.type[i].type} > {this.state.type[i].type}</option >)


        }
        return (tab)
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
                className="table_corection_add_o"

            />
        )
    }
    goback1 = (event) => {
        this.props.history.push('/')
    }
    goback2 = (event) => {
        this.props.history.push('/home')
    }

    render() {
        let x = this.colorsSelector()
        let y = this.typeSelector()
        let table = this.table();
        if (sessionStorage.getItem('valid') === '') {
            return (
                <div className="HomePage">
                    <h1>Log in to have access!</h1>
                    <button type="submit" className="success_login2" onClick={this.goback1} >Log in</button>
                </div>
            );
        }
        else if (sessionStorage.getItem('orderManagement') === 'true' || sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true' || sessionStorage.getItem('admin') === 'true') {
            return (
                <div>
                    <Sidebar />
                    <div className="title">
                        <h1 className="titletext">Add items</h1>
                    </div>
                    <div className="AddOrder2_c">
                        <form>

                            <div className="form-group">

                                <label>Length</label>
                                <input
                                    type="number"
                                    className="form-control"
                                    min="1"
                                    placeholder="Enter length"
                                    id="inputHeight"
                                    ref="length"
                                />
                            </div>
                            <div className="form-group">
                                <label>Width</label>
                                <input
                                    type="number"
                                    className="form-control"
                                    min="1"
                                    placeholder="Enter width"
                                    id="inputWidth"
                                    ref="width"
                                />
                            </div>
                            <div className="form-group">
                                <label>Thickness</label>
                                <input
                                    type="number"
                                    className="form-control"
                                    min="1"
                                    placeholder="Enter thickness"
                                    id="inputThickness"
                                    ref="thickness"
                                />
                            </div>
                            <div className="form-group">
                                <label>Color</label>
                                <select ref="color" type="text" className="form-control">
                                    {x}
                                </select>
                            </div>
                            <div className="form-group">
                                <label>Amount</label>
                                <input
                                    type="number"
                                    min="1"
                                    className="form-control"
                                    id="inputLogin"
                                    placeholder="Enter amount"
                                    ref="amount"
                                />
                            </div>
                            <div className="form-group">
                                <label>Type</label>
                                <select ref="type" type="text" className="form-control">
                                    {y}
                                </select>
                            </div>

                            <button type="submit" className="success_order2_1" onClick={this.addItem}>Add item</button>

                            <button type="submit" className="danger_order2" onClick={this.cancelAdding}>Cancel order</button>

                        </form>

                        <div className="ordertable">
                            <button type="submit" className="success_order2_2" onClick={this.handleAddOrder}>Add</button>
                            {table}
                        </div>

                    </div>

                </div>
            )
        }
        else {
            return (
                <div className="HomePage">
                    <h1>Check if you have perrmission to this panel</h1>
                    <button type="submit" className="success_login2" onClick={this.goback2} >Back to home page</button>
                </div>
            );
        }
    }

}