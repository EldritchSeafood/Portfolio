<template>
    <v-container fluid>
        <v-row
        justify="center"
        class="text-center headline indigo--text"
        style="margin-top:2vh;"
        >Previous Orders</v-row>
        <v-row justify="center">
            <v-col class="title text-center" style="color:red">{{status}}</v-col>
        </v-row>
        <div v-if="orders.length > 0">
            <v-row justify="center" style="background-color:lightblue;margin:3vw;">
                <v-col class="text-center headline" cols="2">Id</v-col>
                <v-col cols="6" class="text-center headline">Date</v-col>
            </v-row>
            <v-row>
                <v-col>
                    <v-list style="max-height: 50vh;margin-top:-3vh;" class="overflow-y-auto">
                        <v-list-item-group>
                            <v-list-item
                            v-for="(order, i) in orders"
                            :key="i"
                            style="color:blue;margin-left:3vw;margin-right:3vw;"
                            @click="popDialog(order)"
                            >
                            <v-col cols="3">
                                <v-list-item-content>
                                    <v-list-item-title class="subtitle-2" v-text="order.id">></v-list-item-title>
                                </v-list-item-content>
                            </v-col>
                            <v-col cols="9">
                                <v-list-item-content>
                                    <v-list-item-title
                                    class="sub-title2 text-left"
                                    v-text="formatDate(order.orderDate)"
                                    >></v-list-item-title>
                                </v-list-item-content>
                            </v-col>
                            </v-list-item>
                        </v-list-item-group>
                    </v-list>
                </v-col>
            </v-row>
            <v-dialog v-model="dialog" v-if="selectedOrder" justify="center" align="center">
                <v-card>
                    <v-row>
                        <v-spacer></v-spacer>
                        <v-btn text @click="dialog = false" style="margin:2vw;">X</v-btn>
                    </v-row>
                    <v-row
                    justify="center"
                    align="center"
                    style="margin-left:3vw;margin-right:3vw;color:blue"
                    >
                    <v-col>Order#{{selectedOrder.id}}</v-col>
                    <v-col v-text="formatDate(selectedOrder.orderDate)"></v-col>
                    </v-row>
                    <v-row>
                        <v-col justify="center" align="center">
                        <v-img
                        :src="require('../assets/cart.png')"
                        style="height:7vh;width:10vh;padding:0;"
                        aspect-ratio="1"
                        />
                        </v-col>
                    </v-row>
                    <div style="margin:2vw;">
                        <v-row justify="center" style="color:purple;margin-bottom:-3vh;font-size:90%;fontWeight:bold">
                            <v-col cols="6"></v-col>
                            <v-col cols="3" class="text-center">Quantities</v-col>
                            <v-col cols="3"></v-col>
                        </v-row>
                        <v-row justify="center" style="color:purple;font-size:90%;fontWeight:bold">
                            <v-col cols="3" class="text-center">Name</v-col>
                            <v-col cols="3" class="text-center">MSRP</v-col>
                            <v-col cols="1" class="text-center">S</v-col>
                            <v-col cols="1" class="text-center">O</v-col>
                            <v-col cols="1" class="text-center">B</v-col>
                            <v-col cols="3" class="text-centers">Extended</v-col>
                        </v-row>
                        <v-row
                        v-for="(detail, i) in details"
                        :key="i"
                        style="margin-bottom:0;margin-top:-2vh;font-size:75%;color:blue"
                        >
                            <v-col cols="3">{{detail.productName}}</v-col>
                            <v-col cols="3" class="text-center">{{detail.msrp | currency}}</v-col>
                            <v-col cols="1" class="text-center">{{detail.qtyOrdered}}</v-col>
                            <v-col cols="1" class="text-center">{{detail.qtySold}}</v-col>
                            <v-col cols="1" class="text-center">{{detail.qtyBackOrdered}}</v-col>
                            <v-col cols="3" class="text-center">{{detail.extPrice | currency}}</v-col>
                        </v-row>
                    </div>
                    <div style="margin:2vw;padding-right:3vw;font-size:75%;color:blue">
                        <v-row style="margin-bottom:0;margin-top:-2vh;">
                            <v-col cols="9" class="text-right">Total:</v-col>
                            <v-col cols="3" class="text-right">{{subTot | currency}}</v-col>
                        </v-row>
                        <v-row style="margin-bottom:0;margin-top:-2vh;">
                            <v-col cols="9" class="text-right">Tax:</v-col>
                            <v-col cols="3" class="text-right">{{taxTot | currency}}</v-col>
                        </v-row>
                        <v-row style="margin-bottom:0;margin-top:-2vh;color:purple;fontWeight:bold">
                            <v-col cols="9" class="text-right">Order Total:</v-col>
                            <v-col cols="3" class="text-right">{{selectedOrder.orderAmount | currency}}</v-col>
                        </v-row>
                    </div>
                    <v-row justify="center" align="center" style="padding-bottom:5vh;">{{this.dialogStatus}}</v-row>
                </v-card>
            </v-dialog>
        </div>
    </v-container>
</template>

<script>
    import fetcher from "../mixins/fetcher";
    import datertn from "../mixins/datertn";
    export default {
        name: "OrderList",
        data() {
            return {
                orders: [],
                status: {},
                details: [],
                selectedOrder: {},
                dialog: false,
                dialogStatus: {},
                subTot: 0,
                taxTot: 0
            };
        },
        mixins: [fetcher, datertn],
        beforeMount: async function() {
            try {
            let user = JSON.parse(sessionStorage.getItem("user"));
            this.status = "fetching orders from server...";
            let route = this.$_buildRouteWithParams("order", user.email.trimEnd());
            this.orders = await this.$_getdata(route.slice(0, -1)); // in mixin
            this.status = `loaded ${this.orders.length} orders`;
            } catch (err) {
                console.log(err);
                this.status = `Error has occured: ${err.message}`;
            }
        },
        methods: {
            selectOrder: async function(orderid) {
                if (orderid > 0) {
                    // don't use arrow function here
                    try {
                        let user = JSON.parse(sessionStorage.getItem("user"));
                        this.status = `fetching details for order ${orderid}...`;
                        let route = this.$_buildRouteWithParams(
                        "order",
                        orderid,
                        user.email.trimEnd()
                        );
                        this.details = await this.$_getdata(route.slice(0, -1)); // remove end /

                        this.details.map(detail => {
                            this.subTot += detail.msrp * detail.qtyOrdered;
                            this.taxTot += (detail.msrp * 0.13) * detail.qtyOrdered;
                        });

                        this.status = `found order ${orderid} details`;
                    } catch (err) {
                        console.log(err);
                        this.status = `Error has occured: ${err.message}`;
                    }
                }
            },
            popDialog: async function(order) {
                this.dialogStatus = "";
                this.dialog = !this.dialog;
                this.selectedOrder = order;
                await this.selectOrder(order.id);
            }
        }
    };
</script>