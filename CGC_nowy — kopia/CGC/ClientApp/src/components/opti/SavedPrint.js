import React, { Component } from "react";
import './SavedPrint.css';
import { ItemsTable } from '../saveProjectTables/ItemsTable'
import { GlassTableProject } from '../saveProjectTables/glassTableProject'
import Sidebar from '../Sidebar';

export class SavedPrint extends Component {
    constructor(props) {
        super(props);
        this.state = {
            value: '',
            value2: '',
            table: [],
            pieces: [],
            glass_ids: []

        }
    }

    componentDidMount() {
        const receiver = {
            order: {
                id_order: sessionStorage.getItem('orderId2'),
            },
            id: sessionStorage.getItem('cutId2'),
        }
           fetch(`api/Cut/Return_Porject`, {
            method: "post",
            body: JSON.stringify(receiver),
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {
                console.log(json)
                var table2 = [];
                table2 = json
                for (var i = 0; i < table2.length; i++) {
                    //table2.push({id:json[i].glass_info[0].id})
                    var canvas = document.getElementById('canvas');
                    if (canvas.getContext) {
                        var ctx = canvas.getContext('2d');
                        if ((table2[i].width >= 100000 && table2[i].width <= 999999) || (table2[i].length >= 100000 && table2[i].length <= 999999)) {
                            if (i == 0) {
                                ctx.fillStyle = 'rgba(9, 157, 215, 0.7)';
                                ctx.fillRect(0, 100, (table2[i].width / 10), (table2[i].length / 10));

                                if (json[i].width === 0) {
                                    sessionStorage.setItem('uncat', json[i].error_Messege)
                                }
                                else {
                                    for (var j = 0; j < json[i].glass_info[0].pieces.length; j++) {
                                        ctx.strokeRect(json[i].glass_info[0].pieces[j].x / 10, (json[i].glass_info[0].pieces[j].y / 10) + 100, json[i].glass_info[0].pieces[j].widht / 10, json[i].glass_info[0].pieces[j].lenght / 10);///itemy
                                    }
                                }
                                this.setState({
                                    position: 100 + (table2[i].length / 10)
                                })

                            }
                            else {
                                ctx.fillStyle = 'rgba(9, 157, 215, 0.7)';
                                ctx.fillRect(0, this.state.position + 200, (table2[i].width / 10), (table2[i].length / 10));

                                if (json[i].width === 0) {
                                    sessionStorage.setItem('uncat', json[i].error_Messege)
                                }
                                else {
                                    for (var j = 0; j < json[i].glass_info[0].pieces.length; j++) {
                                        ctx.strokeRect(json[i].glass_info[0].pieces[j].x / 10, (((json[i].glass_info[0].pieces[j].y / 10) + this.state.position)) + 200, json[i].glass_info[0].pieces[j].widht / 10, json[i].glass_info[0].pieces[j].lenght / 10);///itemy
                                    }
                                }
                                this.setState({
                                    position: this.state.position + 200 + (table2[i].length / 10)
                                })

                            }

                        }
                        else if ((table2[i].width >= 10000 && table2[i].width <= 99999) || (table2[i].length >= 10000 && table2[i].length <= 99999)) {
                            if (i == 0) {
                                ctx.fillStyle = 'rgba(9, 157, 215, 0.7)';
                                ctx.fillRect(0, 100, (table2[i].width / 10), (table2[i].length / 10));

                                if (json[i].width === 0) {
                                    sessionStorage.setItem('uncat', json[i].error_Messege)
                                }
                                else {
                                    for (var j = 0; j < json[i].glass_info[0].pieces.length; j++) {
                                        ctx.strokeRect(json[i].glass_info[0].pieces[j].x / 10, (json[i].glass_info[0].pieces[j].y / 10) + 100, json[i].glass_info[0].pieces[j].widht / 10, json[i].glass_info[0].pieces[j].lenght / 10);///itemy
                                    }
                                }
                                this.setState({
                                    position: 100 + (table2[i].length / 10)
                                })

                            }
                            else {
                                ctx.fillStyle = 'rgba(9, 157, 215, 0.7)';
                                ctx.fillRect(0, this.state.position + 200, (table2[i].width / 10), (table2[i].length / 10));

                                if (json[i].width === 0) {
                                    sessionStorage.setItem('uncat', json[i].error_Messege)
                                }
                                else {
                                    for (var j = 0; j < json[i].glass_info[0].pieces.length; j++) {
                                        ctx.strokeRect(json[i].glass_info[0].pieces[j].x / 10, (((json[i].glass_info[0].pieces[j].y / 10) + this.state.position)) + 200, json[i].glass_info[0].pieces[j].widht / 10, json[i].glass_info[0].pieces[j].lenght / 10);///itemy
                                    }
                                }
                                this.setState({
                                    position: this.state.position + 200 + (table2[i].length / 10)
                                })
                            }
                        }
                        else if ((table2[i].width >= 999 && table2[i].width <= 9999) || (table2[i].length >= 999 && table2[i].length <= 9999)) {
                            if (i == 0) {
                                ctx.fillStyle = 'rgba(9, 157, 215, 0.7)';
                                ctx.fillRect(0, 100, (table2[i].width / 10), (table2[i].length / 10));

                                if (json[i].width === 0) {
                                    sessionStorage.setItem('uncat', json[i].error_Messege)
                                }
                                else {
                                    for (var j = 0; j < json[i].glass_info[0].pieces.length; j++) {
                                        ctx.strokeRect(json[i].glass_info[0].pieces[j].x / 10, (json[i].glass_info[0].pieces[j].y / 10) + 100, json[i].glass_info[0].pieces[j].widht / 10, json[i].glass_info[0].pieces[j].lenght / 10);///itemy
                                    }
                                }
                                this.setState({
                                    position: 100 + (table2[i].length / 10)
                                })

                            }
                            else {
                                ctx.fillStyle = 'rgba(9, 157, 215, 0.7)';
                                ctx.fillRect(0, this.state.position + 200, (table2[i].width / 10), (table2[i].length / 10));

                                if (json[i].width === 0) {
                                    sessionStorage.setItem('uncat', json[i].error_Messege)
                                }
                                else {
                                    console.log('table2[i].width >= 999 && table2[i].width <= 9999) || (table2[i].length >= 999 && table2[i].length <= 9999)')
                                    console.log(this.state.position)
                                    for (var j = 0; j < json[i].glass_info[0].pieces.length; j++) {
                                        ctx.strokeRect(json[i].glass_info[0].pieces[j].x / 10, (((json[i].glass_info[0].pieces[j].y / 10) + this.state.position)) + 200, json[i].glass_info[0].pieces[j].widht / 10, json[i].glass_info[0].pieces[j].lenght / 10);///itemy


                                        /* var yse = (json[i].glass_info[0].pieces[j].y + this.state.position) + 200;
                                         var wid = json[i].glass_info[0].pieces[j].widht / 10;
                                         var len = json[i].glass_info[0].pieces[j].lenght / 10; 
                                         console.log('x=' + json[i].glass_info[0].pieces[j].x / 10 + ' ' + 'y= ' + yse + ' ' + 'width= ' + wid + ' ' + 'lenght=' + len)*/
                                    }
                                }
                                this.setState({
                                    position: this.state.position + 200 + (table2[i].length / 10)
                                })
                            }
                        }
                        else {
                            if (i == 0) {
                                ctx.fillStyle = 'rgba(9, 157, 215, 0.7)';
                                ctx.fillRect(0, 100, (table2[i].width), (table2[i].length));

                                if (json[i].width === 0) {
                                    sessionStorage.setItem('uncat', json[i].error_Messege)
                                }
                                else {
                                    for (var j = 0; j < json[i].glass_info[0].pieces.length; j++) {
                                        ctx.strokeRect(json[i].glass_info[0].pieces[j].x, json[i].glass_info[0].pieces[j].y + 100, json[i].glass_info[0].pieces[j].widht, json[i].glass_info[0].pieces[j].lenght);///itemy
                                    }
                                }
                                this.setState({
                                    position: 100 + (table2[i].length / 10)
                                })

                            }
                            else {
                                ctx.fillStyle = 'rgba(9, 157, 215, 0.7)';
                                ctx.fillRect(0, this.state.position + 200, (table2[i].width / 10), (table2[i].length / 10));

                                if (json[i].width === 0) {
                                    sessionStorage.setItem('uncat', json[i].error_Messege)
                                }
                                else {
                                    for (var j = 0; j < json[i].glass_info[0].pieces.length; j++) {
                                        ctx.strokeRect(json[i].glass_info[0].pieces[j].x, (((json[i].glass_info[0].pieces[j].y / 10) + this.state.position)) + 200, json[i].glass_info[0].pieces[j].widht, json[i].glass_info[0].pieces[j].lenght);///itemy
                                    }
                                }
                                this.setState({
                                    position: this.state.position + 200 + (table2[i].length / 10)
                                })


                            }
                        }
                        ///////////////////// Koniec rysowania tafli
                        ///////////////////// Rysowanie itemów
                        /*if (json[i].width === 0) {
                            sessionStorage.setItem('uncat', json[i].error_Messege)

                        }
                        else {
                            for (var j = 0; j < json[i].glass_info[0].pieces.length; j++) {
                                ctx.strokeRect(json[i].glass_info[0].pieces[j].x, json[i].glass_info[0].pieces[j].y + (2000 * i), json[i].glass_info[0].pieces[j].widht, json[i].glass_info[0].pieces[j].lenght);///itemy
                            }
                        }*/
                    }
                };

                this.setState({
                    glass_ids: table2
                })
                //return (json)
            })
    }


    cutOrder = (event) => {
        event.preventDefault();
       /* const receiver = {
            glasses:
                this.state.glass_ids
            ,
            order: {
                id_order: sessionStorage.getItem('idOpti')
            }

        }


        fetch(`api/Cut/Save_Project`, {
            method: "post",
            body: JSON.stringify(receiver),
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {
                console.log(receiver)
                console.log(json)
                return (json)
            })
            .then(json => {
                sessionStorage.setItem('id_order', json);
            })
        this.props.history.push('/pick_machine');

        console.log(receiver)*/
    }

    //api/Cut/Save_Project
    saveProject = (event) => {
        event.preventDefault();
        this.props.history.push('/saved_projects')

    }


    render() {
        if (sessionStorage.getItem('valid') === '') {
            return (
                <div className="HomePage">
                    <h1>Log in to have access!</h1>
                    <button type="submit" className="success_login" onClick={this.goback} >Log in</button>
                </div>
            );
        }
        else if (sessionStorage.getItem('cutManagement') === 'true' || sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true' || sessionStorage.getItem('admin') === 'true') {
            return (
                <div>
                    <Sidebar />
                    <div className="title">
                        <h1 className="titletext">Saved projects</h1>
                    </div>
                    <div className="SavedPrint_c">
                        <div className="canva">
                            <canvas className="canvas" id="canvas" width="10000" height="10000" ></canvas>
                        </div>
                        <div className="table2">
                            <h2>Glasses</h2>
                            <GlassTableProject />
                        </div>
                        <div className="table3">
                            <h2>Products</h2>
                            <ItemsTable />
                        </div>
                        <div>
                            <button className="prim_test" onClick={this.saveProject}>Back</button>
                            <button className="success_test" onClick={this.cutOrder}>Cut</button>

                        </div>


                    </div>
                </div>
            );
        }
        else {
            return (
                <div>
                    <Sidebar />
                    <div className="title">
                        <h1 className="titletext">Saved projects</h1>
                    </div>
                    <div className="SavedPrint_c">
                        <div className="canva">
                            <canvas className="canvas" id="canvas" width="10000" height="10000" ></canvas>
                        </div>
                        <div className="table2">
                            <h2>Glasses</h2>
                            <GlassTableProject />
                        </div>
                        <div className="table3">
                            <h2>Products</h2>
                            <ItemsTable />
                        </div>
                        <div>
                            <button className="prim_test" onClick={this.saveProject}>Back</button>
                        </div>
                    </div>
                </div>
            );
        }
    }
}